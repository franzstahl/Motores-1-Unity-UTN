using UnityEngine;

[RequireComponent(typeof(CharacterController))] // Ensures the GameObject this script is attached to has a CharacterController component. 
public class playerMovement : MonoBehaviour
{
    private CharacterController controller;
    private InputSystem_Actions inputActions; // This allows us to access the input actions defined in the Input System
    private Vector2 moveInput;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
