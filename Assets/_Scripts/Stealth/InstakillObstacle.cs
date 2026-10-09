using System.Collections;
using UnityEngine;

public class InstakillObstacle : Obstacle
{
    public override void ApplyColliderType(Collider collider)
    {
        Debug.Log("Be sure to check that it either doesn't have gravity, or that it's Y position is locked. Otherwise, it's gonna be falling for eternity in the void, you shmuck");
        collider.isTrigger = true;
    }
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
