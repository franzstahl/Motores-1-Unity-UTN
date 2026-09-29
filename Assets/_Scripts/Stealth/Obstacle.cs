using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;
    NoiseMeter noiseMeter;
    [SerializeField] private bool causesInstantLoss = false;
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        noiseMeter = gameManager.GetComponent<NoiseMeter>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (causesInstantLoss && collision.gameObject.tag == "Player")
        {
            gameManager.PlayerDetected = true;
        }
        if(collision.gameObject.tag == "Ground")
        {
            noiseMeter.IncreaseNoiseMeter();
        }
    }
}
