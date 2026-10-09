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
