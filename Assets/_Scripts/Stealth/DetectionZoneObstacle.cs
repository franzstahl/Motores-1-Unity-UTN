using System.Collections;
using UnityEngine;

public class DetectionZoneObstacle : Obstacle
{
    private float timeDetectionZoneLasts;
    private float timer;
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

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            // deactivate gameObject.GetComponent<BoxCollider>();
        }
    }

    private IEnumerator DetectionZoneTimer()
    {
        yield return timeDetectionZoneLasts;
    }
}
