using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadScenes : MonoBehaviour
{
    [SerializeField] private MonoBehaviour movementScript;
    [SerializeField] private MonoBehaviour playerCamera;
    [SerializeField] private PlayerInput playerInput;
    private bool isPaused;
    public void StartGame()
    {
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (playerInput.actions["Pause"].triggered)
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused == true)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (movementScript != null )
        {
            movementScript.enabled = false;
        }
        if (playerCamera != null )
        {
            playerCamera.enabled = false;
        }
    }
    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
        if (playerCamera != null)
        {
            playerCamera.enabled = true;
        }
    }
}
