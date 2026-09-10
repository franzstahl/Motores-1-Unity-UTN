using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public Transform shootPoint;
    [SerializeField] private float maxDistance;

    public void Shoot()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;
        Debug.DrawRay(shootPoint.position, shootPoint.forward * maxDistance, Color.yellow);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if(hit.collider.GetComponent<InteractibleInterface>()  != null)
                hit.collider.GetComponent<InteractibleInterface>().Interact();
        }
    }
}
