using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.PlayerDetected = true;
        }
    }
}
