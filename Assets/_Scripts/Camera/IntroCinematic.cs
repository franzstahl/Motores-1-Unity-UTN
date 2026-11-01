using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class IntroCinematic : MonoBehaviour
{
    [SerializeField] private GameObject cinematicCamera;
    [SerializeField] private GameObject hud; // Starts disabled in the editor, enabled when the cinematic ends
    [SerializeField] private float holdDuration = 3f; // Seconds the fridge stays on screen
    [SerializeField] private float blendDuration = 6f; // Should match the Brain's Default Blend
    [SerializeField] private float closedHoldDuration = 2f; // Seconds the closed fridge stays on screen before the camera moves
    [SerializeField] private CameraCollision cameraCollision;
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private Fridge fridge; // To close it during the camera cut
    [SerializeField] private CanvasGroup menuGroup; // CanvasGroup on the main menu canvas
    [SerializeField] private LoadScenes loadScenes; // The menu's LoadScenes, called after the fade
    [SerializeField] private float menuFadeDuration = 0.5f;
    private IEnumerator Start()
    {
        yield return null; // Wait one frame so GameManager.Start has already run
        Cursor.lockState = CursorLockMode.None; // Free the cursor while the main menu is open
        Cursor.visible = true;
        GameManager.Instance.SetMovementActive(false); // Locked while the main menu is open
    }

    public void PlayIntro() // Called by the Play button
    {
        StartCoroutine(IntroRoutine());
    }

    private IEnumerator IntroRoutine()
    {
        menuGroup.interactable = false; // No double clicks while fading
        float t = 0f;
        while (t < menuFadeDuration)
        {
            t += Time.unscaledDeltaTime; // Unscaled: the game is still paused (timeScale = 0) while the menu is open
            menuGroup.alpha = 1f - t / menuFadeDuration;
            yield return null;
        }
        menuGroup.alpha = 0f;

        loadScenes.StartGame(); // Hides the menu and resumes the game, now that it has faded out
        yield return new WaitForSeconds(holdDuration); // Time on the fridge after pressing Play

        fridge.CloseInstantly(); // Closed in the same frame as the camera cut
        yield return new WaitForSeconds(closedHoldDuration);

        cinematicCamera.SetActive(false);
        yield return new WaitForSeconds(blendDuration);

        cameraCollision.enabled = true; // Gives the camera to the player (its Start turns off the Brain)
        hud.SetActive(true);
        GameManager.Instance.SetMovementActive(true);
    }
    
}
