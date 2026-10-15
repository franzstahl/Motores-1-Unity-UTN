using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Padlock : MonoBehaviour, InteractibleInterface
{
    private Rigidbody rb;

    public bool isUnlocked { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Interact()
    {
        if (isUnlocked) return; // Already on te floor, do nothing

        if (GameManager.Instance == null || !GameManager.Instance.hasKey)
        {
            Debug.Log("You need a key to unlock this padlock.");
            return;
        }

        isUnlocked = true;
        rb.isKinematic = false; // Falling the padlock when unlocked
    }
}
