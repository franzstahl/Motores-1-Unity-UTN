using UnityEngine;

public class Fridge : MonoBehaviour, InteractibleInterface
{
    //[SerializeField] private GameObject openFridge;
    //[SerializeField] private Vector3 prefabPosition;
    //[SerializeField] private Quaternion prefabRotation;

    [SerializeField] private Padlock padlock;
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open"; // Name of the trigger parameter in the Animator
    [SerializeField] private Light fridgeLight; // Interior light, must start disabled

    private bool isOpen;
    public void Interact() // Handles the interaction with the fridge object
    {
        if (isOpen) return;

        if (padlock != null && !padlock.isUnlocked) // Check if the fridge is locked by the padlock
        {
            Debug.Log("The fridge is locked.");
            return;
        }

        isOpen = true;

        if (animator != null)
        {
            animator.SetTrigger(openTrigger); // Trigger the opening animation
        }

        TurnOnLight();

        //GameObject clone;
        //clone = Instantiate(openFridge, prefabPosition, prefabRotation);
        //Destroy(gameObject);
    }

    public void TurnOnLight() // Public so an Animation Event can also call it at a specific frame
    {
        if (fridgeLight != null)
        {
            fridgeLight.enabled = true;
        }
    }
}
