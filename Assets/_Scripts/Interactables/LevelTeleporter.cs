using UnityEngine;

public class LevelTeleporter : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject player;

    public void Interact()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = destination.position;
        player.GetComponent<CharacterController>().enabled = true;
    }
}