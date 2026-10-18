using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    protected GameManager gameManager;
    protected NoiseMeter noiseMeter;
    public GameObject destroyParticle;
    private bool hasMadeNoise;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private void Start()
    {
        gameManager = GameManager.Instance;
        noiseMeter = gameManager.GetComponent<NoiseMeter>();
        rb = GetComponent<Rigidbody>();
        rb.mass = 1f;
        Collider col = gameObject.GetComponent<Collider>();
        ApplyColliderType(col);
            
        startPosition = transform.position;
        startRotation = transform.rotation;

        gameManager.OnPlayerRespawn += ResetObstacle; // Subscribe to the OnPlayerRespawn event to reset the obstacle when the player respawns
    }

    public virtual void ApplyColliderType(Collider collider) // Called in Start() to set the collider type for the obstacle
    {
        collider.isTrigger = false;
    }
    
    public virtual void ObstacleEffect(Collider objectCollidedWith) // Called when the obstacle collides with another object
    {
        if (hasMadeNoise) return; // Prevents the obstacle from making noise multiple times
        if (objectCollidedWith.CompareTag("Ground"))
        {
            hasMadeNoise = true;
            noiseMeter.IncreaseNoiseMeter();
            objectCollidedWith.GetComponent<AudioSource>().Play();
            GameObject go = Instantiate(destroyParticle, gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(go, 1f);
            Destroy(gameObject, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision) // Called when the obstacle collides with another object
    {
        ObstacleEffect(collision.collider);
    }

    private void ResetObstacle() // Resets the obstacle to its original position and rotation, and stops any movement
    {
        hasMadeNoise = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(startPosition, startRotation);
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnPlayerRespawn -= ResetObstacle; // Unsubscribe from the OnPlayerRespawn event when the obstacle is destroyed
        }
    }
}
