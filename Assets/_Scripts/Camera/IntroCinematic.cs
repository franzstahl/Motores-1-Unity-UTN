using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class IntroCinematic : MonoBehaviour
{
    [SerializeField] private GameObject cinematicCamera;
    [SerializeField] private GameObject hud; // Starts disabled in the editor, enabled when the cinematic ends
    [SerializeField] private float holdDuration = 3f; // Seconds the fridge stays on screen
    [SerializeField] private float blendDuration = 6f; // Should match the Brain's Default Blend
    [SerializeField] private CameraCollision cameraCollision;
    [SerializeField] private CinemachineBrain brain;

    private IEnumerator Start()
    {
        yield return null; // Wait one frame so GameManager.Start has already run
        GameManager.Instance.SetMovementActive(false); // Locked while the main menu is open
    }

    public void PlayIntro() // Called by the Play button
    {
        StartCoroutine(IntroRoutine());
        Debug.Log("PlayIntro called"); // Temporary, remove after debugging
    }

    private IEnumerator IntroRoutine()
    {
        yield return new WaitForSeconds(holdDuration); // Time on the fridge after pressing Play

        cinematicCamera.SetActive(false);
        yield return new WaitForSeconds(blendDuration);

        cameraCollision.enabled = true; // Gives the camera to the player (its Start turns off the Brain)
        hud.SetActive(true);
        GameManager.Instance.SetMovementActive(true);
    }
}
