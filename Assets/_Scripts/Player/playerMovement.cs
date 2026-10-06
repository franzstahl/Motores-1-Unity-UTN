using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState { Walking, Sprinting, Climbing } // Movement states the player can be in. Public and outside the class so other scripts can read it.

[RequireComponent(typeof(CharacterController))] // Ensures the GameObject this script is attached to has a CharacterController component. 
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float rotationSpeed = 10f; // Speed at which the player rotates to face the movement direction
    [SerializeField] private float sprintSpeed = 5.5f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainPerSecond = 25f; // Lost per second while sprinting
    [SerializeField] private float staminaRegenPerSecond = 15f; // Gained per second while not sprinting
    [SerializeField] private float regenDelay = 1f; // Seconds to wait after sprinting before regenerating
    [SerializeField] private float minStaminaToSprint = 20f; // After running out, stamina needed to sprint again

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 3f; // Speed while moving along the wall
    [SerializeField] private float climbDuration = 4f; // Max seconds the player can stay on a wall
    [SerializeField] private float wallCheckDistance = 0.6f; // How far the wall ray looks
    [SerializeField] private string climbableTag = "Climbable"; // Walls with this tag can be climbed

    private float climbTimer; // Time left on the wall
    private Vector3 wallNormal; // Points away from the wall, toward the player
    private bool canClimb = true; // Set to false after leaving a wall, reset when touching the ground
    private float currentStamina;
    private float regenTimer;
    private bool exhausted; // True after hitting 0, until stamina recovers past the minimum

    public float StaminaNormalized => currentStamina / maxStamina; // 0 to 1, for the UI later

    private bool sprintHeld; // True while the sprint key is being held down
    public MovementState CurrentState { get; private set; } = MovementState.Walking; // Other scripts can read it but not change it

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private InputSystem_Actions inputActions; // This allows us to access the input actions defined in the Input System
    private Vector2 moveInput;
    private bool jumpRequested;
    private Vector3 velocity; // This will hold player's current velocity, including vertical movement due to gravity and jumping
    private bool isGrounded;

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
        currentStamina = maxStamina; // Start with full stamina
    }

    private void Start()
    {
        gameManager = GameManager.Instance; // Singleton access, no scene search needed
        if (cameraTransform == null && Camera.main != null) // If no cameraTransform is assigned in the inspector, try to find the main camera in the scene.
            cameraTransform = Camera.main.transform;
    }

    private void OnEnable() // Turns on the input actions and connects the functions to the Move and Jump events
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Sprint.performed += OnSprintPerformed;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;

    }

    private void OnDisable() // Turns on the input actions and connects the functions to the Move and Jump events
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Sprint.performed -= OnSprintPerformed;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        inputActions.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context) // Save where you are moving (Vector2) every time you press a movement key
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) // Reset the movement to zero when you release the keys
    {
        moveInput = Vector2.zero; 
    }

    private void OnSprintPerformed(InputAction.CallbackContext context) { sprintHeld = true; }
    private void OnSprintCanceled(InputAction.CallbackContext context) { sprintHeld = false; }
    private void OnJumpPerformed(InputAction.CallbackContext context) // Mark that was requested to be jumped (a flag), to be processed later
    {
        if (gameManager.isMovementActive)
            jumpRequested = true;
    }

    private void Update()
    {
        HandleGroundCheck();

        if (CurrentState == MovementState.Climbing) // While climbing, skip normal movement, jump and gravity
        {
            HandleClimb();
            return;
        }

        HandleMovement();
        HandleJump();
        ApplyGravity();
        TryStartClimb();
    }

    private void HandleGroundCheck() // Check if player is touching ground
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (isGrounded)
            canClimb = true; // Reset climbing ability when touching the ground
    }

    private Vector3 FeetRayOrigin() // Ray origin near the feet, so the ray stops hitting when the feet pass the top edge
    {
        Vector3 center = transform.TransformPoint(controller.center);
        return center + Vector3.down * (controller.height * 0.5f - 0.1f);
    }

    private void TryStartClimb() // Grab a climbable wall
    {
        if (isGrounded || !canClimb || moveInput.sqrMagnitude < 0.01f) return;

        if (Physics.Raycast(FeetRayOrigin(), transform.forward, out RaycastHit hit, wallCheckDistance, ~0, QueryTriggerInteraction.Ignore) && hit.collider.CompareTag(climbableTag))

        {
            CurrentState = MovementState.Climbing;
            climbTimer = climbDuration;
            wallNormal = new Vector3(hit.normal.x, 0f, hit.normal.z).normalized; // Flatten, we assume vertical walls
            velocity = Vector3.zero; // Cancel any falling or jumping speed
        }
    }

    private void HandleClimb() // Runs every frame instead of the normal movement while climbing
    {
        climbTimer -= Time.deltaTime;

        if (jumpRequested) // Jump off the wall
        {
            jumpRequested = false;
            StopClimb();
            controller.Move(wallNormal * 0.3f); // Small push away from the wall
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            return;
        }

        bool wallAhead = Physics.Raycast(FeetRayOrigin(), -wallNormal, wallCheckDistance,
                                         ~0, QueryTriggerInteraction.Ignore); // Check if the wall is still in front of the player, using the feet ray origin so it stops hitting when the feet pass the top edge

        if (!wallAhead) // The wall ended: the feet passed the top edge, so push up and forward onto it
        {
            controller.Move(-wallNormal * (controller.radius + 0.3f) + Vector3.up * 0.1f);
            StopClimb();
            return;
        }

        if (climbTimer <= 0f) // Time is up, fall
        {
            StopClimb();
            return;
        }

        transform.rotation = Quaternion.LookRotation(-wallNormal); // Face the wall

        Vector3 wallRight = Vector3.Cross(Vector3.up, -wallNormal); // Sideways direction along the wall
        Vector3 climbMove = (Vector3.up * moveInput.y + wallRight * moveInput.x) * climbSpeed;

        if (gameManager.isMovementActive)
            controller.Move(climbMove * Time.deltaTime);

        if (controller.isGrounded && moveInput.y < 0f) // Went back down to the floor
            StopClimb();
    }

    private void StopClimb() // Gives control back to the normal movement
    {
        CurrentState = MovementState.Walking;
        canClimb = false; // Can't grab again until touching the ground
    }

    private void UpdateStamina(bool isSprinting) // Drains stamina while sprinting and regenerates it after a short delay
    {
        if (isSprinting)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime; // Drain per second, scaled by deltaTime so it's frame rate independent
            regenTimer = regenDelay; // Reset the regen delay every frame we sprint, so it only counts down once we stop

            if (currentStamina <= 0f)
            {
                currentStamina = 0f; // Clamp to 0, the subtraction can overshoot into negative values
                exhausted = true; // Block sprinting until stamina recovers past minStaminaToSprint
            }
        }

        else
        {
            if (regenTimer > 0f)
            {
                regenTimer -= Time.deltaTime; // Still waiting before regeneration starts
            }
            else
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenPerSecond * Time.deltaTime); // Regenerate, never above max
            }
               

            if (exhausted && currentStamina >= minStaminaToSprint)
            {
                exhausted = false; // Recovered enough, sprinting is allowed again
            }
               
        }
    }

    private void HandleMovement() // Converts input into real movement, relative where the camera is looking, and turns character in direction is walking.
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        // Determine if the player wants to sprint and if they can sprint based on stamina
        bool wantsToSprint = sprintHeld && moveInput.sqrMagnitude > 0.01f; // Only sprint if the sprint key is held and the player is moving
        bool isSprinting = wantsToSprint && !exhausted; // Can't sprint while exhausted
        UpdateStamina(isSprinting); // Update stamina every frame

        // Update the current movement state and speed based on whether the player is sprinting or walking
        CurrentState = isSprinting ? MovementState.Sprinting : MovementState.Walking;
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if (gameManager.isMovementActive)
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    } 

    private void HandleJump() // Apply jump velocity
    {
        if (jumpRequested && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        jumpRequested = false;
    }

    private void ApplyGravity() // Accumulate and applies the constant fall
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
