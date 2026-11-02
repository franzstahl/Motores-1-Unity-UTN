using UnityEngine;

public class ClosestPointAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Collider surfaceCollider; // In this case, it's the collider for the ground
    [SerializeField] private Transform listenerTransform;

    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //surfaceCollider = GetComponent<Collider>();
        AudioListener listener = FindAnyObjectByType<AudioListener>();

        if (listener != null)
        {
            listenerTransform = listener.transform;
        }
    }

    
    private void Update()
    {
        if (listenerTransform == null || surfaceCollider == null) return;

        Vector3 closestPoint = surfaceCollider.ClosestPoint(listenerTransform.position);
        audioSource.transform.position = closestPoint;
    }
}
