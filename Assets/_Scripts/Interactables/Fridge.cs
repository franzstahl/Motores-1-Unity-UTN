using UnityEngine;

public class Fridge : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private Padlock padlock;
    [SerializeField] private Animator animator;
    [SerializeField] private string openTrigger = "Open"; // Name of the trigger parameter in the Animator
    [SerializeField] private string openState = "Fridge opened"; // Last state of the chain, the open pose
    [SerializeField] private string closedState = "Fridge closed"; // Default state, the closed pose
    [SerializeField] private Light[] fridgeLights; // Interior light, must start disabled
  
    private bool isOpen;

    private void Start()
    {
        ShowOpenPose(); // The main menu shows the fridge already open
    }
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
        
    }

    public void TurnOnLight() // Public so an Animation Event can also call it at a specific frame
    {
        SetLights(true);
    }

    public void TurnOffLights()
    {
        SetLights(false);
    }

    private void SetLights(bool on) // Helper function to turn on/off the fridge lights
    {
        foreach (Light l in fridgeLights)
        {
            if (l != null) l.enabled = on;
        }
    }
    public void ShowOpenPose() // Jumps to the last frame of the opening state
    {
        if (animator == null) return;
        animator.Play(openState, 0, 1f);
        animator.Update(0f); // Apply the pose right now, don't wait for the next frame
    }

    public void CloseInstantly() // Back to the closed pose with no animation
    {
        if (animator != null)
        {
            animator.Play(closedState, 0, 0f);
            animator.Update(0f);
            animator.ResetTrigger(openTrigger); // So a leftover trigger can't reopen it
        }

        isOpen = false;
        TurnOffLights();
    }
}
