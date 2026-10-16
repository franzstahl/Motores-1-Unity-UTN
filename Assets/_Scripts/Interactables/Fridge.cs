using UnityEngine;

public class Fridge : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private GameObject openFridge;
    [SerializeField] private Vector3 prefabPosition;
    [SerializeField] private Quaternion prefabRotation;

    [SerializeField] private Padlock padlock; // Reference to the Padlock object
    public void Interact() // Handles the interaction with the fridge object
    {
        if (padlock != null && !padlock.isUnlocked) // Check if the fridge is locked by the padlock
        {
            Debug.Log("The fridge is locked.");
            return;
        }

        GameObject clone;
        clone = Instantiate(openFridge, prefabPosition, prefabRotation);
        Destroy(gameObject);
    }
}
