using UnityEngine;

public class Key : MonoBehaviour, InteractibleInterface
{
    public void Interact ()
    {
        GameManager.Instance.CollectKey();
        Destroy(gameObject);

    }
}
