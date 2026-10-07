using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    protected GameManager gameManager;
    protected NoiseMeter noiseMeter;
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        noiseMeter = gameManager.GetComponent<NoiseMeter>();
    }

    public virtual void ObstacleEffect(Collider objectCollidedWith)
    {
        if(objectCollidedWith.gameObject.tag == "Ground")
        {
            noiseMeter.IncreaseNoiseMeter();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        ObstacleEffect(collision);
    }
}
