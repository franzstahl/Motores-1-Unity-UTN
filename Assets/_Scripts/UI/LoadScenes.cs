using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadScenes : MonoBehaviour
{
    [Header("Player scripts")]
    [SerializeField] private MonoBehaviour movementScript;
    [SerializeField] private MonoBehaviour playerCamera;
    [SerializeField] private PlayerInput playerInput;
    [Header("Menu panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [Header("Cameras")]
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject gameCamera;
    private bool isPaused;
    private void Awake()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }
        if (menuCamera != null)
        {
            menuCamera.SetActive(true);
        }
        if (gameCamera != null)
        {
            gameCamera.SetActive(false);
        }
        Pause();
    }
    private void Update()
    {
        if (playerInput.actions["Pause"].triggered)
        {
            TogglePause();
        }
    }
    public void StartGame()
    {
        if (menuCamera != null)
        {
            menuCamera.SetActive(false);
        }
        if (gameCamera != null)
        {
            gameCamera.SetActive(true);
        }
        mainMenu.SetActive(false);
        Resume();
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused == true)
        {
            Pause();
            OpenPauseMenu();
        }
        else
        {
            Resume();
            ClosePauseMenu();
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
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
