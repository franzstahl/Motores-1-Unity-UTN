using UnityEngine;

public class Key : MonoBehaviour, InteractibleInterface
{
    private SFXPlayer sfx2D;
    [SerializeField] private float groundOffset = 0.02f;
    private bool collected;

    private void Start()
    {
        SnapToSurface();
    }

    private void SnapToSurface()
    {
        Renderer r = GetComponentInChildren<Renderer>();
        if (r == null) return;

        Vector3 origin = transform.position + Vector3.up * 1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 3f, ~0, QueryTriggerInteraction.Ignore))
        {
            float bottomOffset = transform.position.y - r.bounds.min.y; // Distance from the pivot to the bottom of the model
            Vector3 pos = transform.position;
            pos.y = hit.point.y + bottomOffset + groundOffset;
            transform.position = pos;
        }
    }

    public void SetSFX(SFXPlayer player)
    {
        sfx2D = player;
    }
    public void Interact ()
    {
        if (collected) return;
        collected = true;

        if (sfx2D != null) sfx2D.Play("KeyPickUp");
        GameManager.Instance.CollectKey();
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }
}
