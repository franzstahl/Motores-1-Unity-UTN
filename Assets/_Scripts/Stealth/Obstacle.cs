using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    protected GameManager gameManager;
    protected NoiseMeter noiseMeter;
    public GameObject destroyParticle;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        noiseMeter = gameManager.GetComponent<NoiseMeter>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = 1f;
        Collider col = gameObject.GetComponent<Collider>();
        ApplyColliderType(col);
    }

    public virtual void ApplyColliderType(Collider collider)
    {
        collider.isTrigger = false;
    }
    
    public virtual void ObstacleEffect(Collider objectCollidedWith) // Called when the obstacle collides with another object
    {
        if (objectCollidedWith.CompareTag("Ground"))
        {
            noiseMeter.IncreaseNoiseMeter();
            objectCollidedWith.GetComponent<AudioSource>().Play();
            GameObject go = Instantiate(destroyParticle, gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(go, 1f);
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        ObstacleEffect(collision);
    }
}
