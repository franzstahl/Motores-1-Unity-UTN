using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheese : MonoBehaviour, InteractibleInterface
{
    private string nextSceneName = "Win";

    
    [SerializeField] private float delayBeforeLoad = 1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip victoryAudio;
    private int interactionsDone;
    [SerializeField] private GameObject[] cheesesToSpawn;
    [SerializeField] private GameObject currentCheese;
    private bool finalInteraction;

    private void Start()
    {
        interactionsDone = 0;
        currentCheese.SetActive(true);
    }
    public void Interact() // Handles the interaction with the cheese object
    {
        if (finalInteraction)
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
        else
        {
            NextCheese();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
    private void NextCheese()
    {
        currentCheese.SetActive(false);
        //play eat cheese noise
        interactionsDone++;
        currentCheese = cheesesToSpawn[interactionsDone];
        currentCheese.SetActive(true);
        if (interactionsDone == cheesesToSpawn.Length)
        {
            finalInteraction = true;
        }
    }
}
