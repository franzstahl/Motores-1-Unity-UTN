using System.Collections;
using UnityEngine;

public class InstakillObstacle : Obstacle
{
    public override void ObstacleEffect(Collider objectCollidedWith)
    {
        if(objectCollidedWith.gameObject.tag == "Player")
        {
            gameManager.EnterDetectedState();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        ObstacleEffect(collision);
    }
}
