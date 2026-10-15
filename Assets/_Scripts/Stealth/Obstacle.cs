using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    protected GameManager gameManager;
    protected NoiseMeter noiseMeter;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        noiseMeter = gameManager.GetComponent<NoiseMeter>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.mass = 0.0000001f;
        Collider col = gameObject.GetComponent<Collider>();
        ApplyColliderType(col);
    }

    public virtual void ApplyColliderType(Collider collider)
    {
        collider.isTrigger = false;
    }

    public virtual void ObstacleEffect(Collider objectCollidedWith)
    {
        if(objectCollidedWith.CompareTag("Ground"))
        {
            noiseMeter.IncreaseNoiseMeter();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        ObstacleEffect(collision);
    }
}
