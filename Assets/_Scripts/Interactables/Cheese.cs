using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheese : MonoBehaviour, InteractibleInterface
{
    private string nextSceneName = "Win";

    
    [SerializeField] private float delayBeforeLoad = 1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip victoryAudio;
    

    public void Interact() // Handles the interaction with the cheese object
    {
        if (GameManager.Instance != null)
        {
            audioSource.PlayOneShot(victoryAudio);
            GameManager.Instance.FadeOut();
            Invoke(nameof(LoadNextScene), delayBeforeLoad);
        }
        else
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
