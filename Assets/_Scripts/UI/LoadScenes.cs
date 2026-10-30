using UnityEngine;
using UnityEngine.InputSystem;

public class LoadScenes : MonoBehaviour
{
    [Header("Player scripts")]
    [SerializeField] private MonoBehaviour movementScript;
    [SerializeField] private MonoBehaviour playerCamera;
    [Header("Menu panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private bool startPaused = true; // Uncheck in scenes without a main menu, so the game doesn't start frozen
    [Header("Cameras")]
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject gameCamera;
    private bool isPaused;
    private void Awake()
    {
        if (!startPaused) return;
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
        // Read the pause key directly, no PlayerInput component needed
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
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
        if (mainMenu != null && mainMenu.activeSelf) return; // Don't open the pause menu over the main menu

        if (isPaused) { Resume(); ClosePauseMenu(); }
        else { Pause(); OpenPauseMenu(); }
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }
    }
    public void Resume()
    {
        isPaused = false;
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
