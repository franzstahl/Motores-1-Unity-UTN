using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private Button button;
    private GameObject instance;
    private UIsounds soundManager;
   
    private void Start()
    {
        instance = GameObject.Find("UISounds");
        soundManager = instance.GetComponent<UIsounds>();

        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }
    public void PlaySound()
    {
        soundManager.PlaySound();
    }
}
