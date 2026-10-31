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

        cinematicCamera.SetActive(true);
        yield return null; // Let the Brain do the cut this frame

        GameManager.Instance.SetMovementActive(false);
        cinematicCamera.SetActive(true);

        yield return new WaitForSeconds(holdDuration);

        cinematicCamera.SetActive(false); // The Brain blends back to the player camera by itself
        yield return new WaitForSeconds(blendDuration); // Wait for the blend to finish

        brain.enabled = false;
        cameraCollision.enabled = true;

        hud.SetActive(true);
        GameManager.Instance.SetMovementActive(true);
    }
}
