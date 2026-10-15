using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private Transform shootPoint;
    [SerializeField] private float maxDistance;

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
