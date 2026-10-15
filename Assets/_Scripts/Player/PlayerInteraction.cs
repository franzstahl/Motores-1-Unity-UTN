using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private Transform shootPoint;
    [SerializeField] private float maxDistance;
    public float pushPower = 2f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Ignore if no rigidbody or it's kinematic
        if (body == null || body.isKinematic)
            return;

        // Don't push things below us (e.g. when standing on them)
        if (hit.moveDirection.y < -0.3f)
            return;

        // Push only horizontally
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.linearVelocity = pushDir * pushPower;   // use linearVelocity in Unity 6+
    }
    public void Shoot() // Handles the raycast shooting logic and interaction with objects
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;
        Debug.DrawRay(shootPoint.position, shootPoint.forward * maxDistance, Color.yellow);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Golpeó: " + hit.collider.gameObject.name);

            if (hit.collider.GetComponent<InteractibleInterface>() != null)
                hit.collider.GetComponent<InteractibleInterface>().Interact();
        }
      
    }
}
