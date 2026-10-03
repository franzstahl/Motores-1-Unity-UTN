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

    private void HandleMovement() // Converts input into real movement, relative where the camera is looking, and turns character in direction is walking.
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

        // Sprint only counts if the player is also moving, so standing still with Shift held stays as Walking
        bool isSprinting = sprintHeld && moveInput.sqrMagnitude > 0.01f;
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
