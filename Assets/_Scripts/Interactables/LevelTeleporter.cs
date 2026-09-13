using UnityEngine;

public class LevelTeleporter : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private Transform destination;

    public void Interact()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = destination.position;
    }
}