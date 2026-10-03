using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState { Walking, Sprinting } // Movement states the player can be in. Public and outside the class so other scripts can read it.

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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleGroundCheck() // Check if player is touching ground
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
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
