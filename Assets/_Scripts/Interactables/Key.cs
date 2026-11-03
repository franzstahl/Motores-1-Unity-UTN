using UnityEngine;

public class Key : MonoBehaviour, InteractibleInterface
{
    [SerializeField] private SFXPlayer sfx2D;
    public void Interact ()
    {
        sfx2D.Play("KeyPickUp");
        GameManager.Instance.CollectKey();
        Destroy(gameObject);

    }
}
