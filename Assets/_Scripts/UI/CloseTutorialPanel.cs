using UnityEngine;
using UnityEngine.InputSystem;

public class CloseTutorialPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelToClose;

    private void Update()
    {
        if (!panelToClose.activeSelf)
            return;

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            panelToClose.SetActive(false);
        }
    }
}
