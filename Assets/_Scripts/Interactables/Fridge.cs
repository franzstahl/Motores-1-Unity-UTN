using UnityEngine;

public class Fridge : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private GameObject openFridge;
    [SerializeField] private Vector3 prefabPosition;
    [SerializeField] private Quaternion prefabRotation;
    public void Interact() // Handles the interaction with the fridge object
    {
        GameObject clone;
        clone = Instantiate(openFridge, prefabPosition, prefabRotation);
        Destroy(gameObject);
    }
}
