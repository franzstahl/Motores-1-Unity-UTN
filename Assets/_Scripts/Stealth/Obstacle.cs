using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.PlayerDetected = true;
        }
    }
}
