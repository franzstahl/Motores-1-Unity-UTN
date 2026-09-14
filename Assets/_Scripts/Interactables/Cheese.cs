using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheese : MonoBehaviour, InteractibleInterface
{
    private string nextSceneName = "Win";

    [SerializeField] private GameManager gameManager;
    [SerializeField] private float delayBeforeLoad = 1f;
    public AudioSource audioSource;
    public AudioClip victoryAudio;
    

    public void Interact() // Handles the interaction with the cheese object
    {
        if (gameManager != null)
        {
            audioSource.PlayOneShot(victoryAudio);
            gameManager.FadeOut();
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
