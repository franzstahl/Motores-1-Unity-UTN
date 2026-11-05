using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheese : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip victoryAudio;
    [SerializeField] private GameObject[] cheesesToSpawn;
    [SerializeField] private GameObject currentCheese;
    [SerializeField] private GameObject endScreen;

    [SerializeField] private SFXPlayer playerSFX3D;

    private bool finalInteraction;
    private int interactionsDone;
    private bool ending;

    private void Start()
    {
        interactionsDone = 0;
        currentCheese.SetActive(true);
    }
    public void Interact() // Handles the interaction with the cheese object
    {
        if (finalInteraction)
        {
            if (ending) return;
            ending = true;

            if (audioSource != null && victoryAudio != null) audioSource.PlayOneShot(victoryAudio);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.EnterEndingState();
            }
            else
            {
                endScreen.SetActive(true);
            }
        }
        else
        {
            NextCheese();
        }
    }

    private void NextCheese()
    {
        currentCheese.SetActive(false);
        playerSFX3D.Play("CheeseBite");
        interactionsDone++;
        currentCheese = cheesesToSpawn[interactionsDone];
        currentCheese.SetActive(true);

        if (interactionsDone == cheesesToSpawn.Length - 1)
        {
            finalInteraction = true;
        }
    }
}
