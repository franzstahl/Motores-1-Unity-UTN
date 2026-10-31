using UnityEngine;

public class KeyLockedBarrier : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private BoxCollider boxCollider;

    private void Start()
    {
        gameManager = GameManager.Instance;
        boxCollider = GetComponent<BoxCollider>();
    }

    
    private void Update()
    {
        // Key is kept after being caught, so this stays open after respawn
        boxCollider.enabled = !(gameManager.hasKey);
    }
}
