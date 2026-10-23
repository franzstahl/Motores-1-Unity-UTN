using UnityEngine;

public class ObjectiveParticles : MonoBehaviour
{
    [SerializeField] private GameObject objective;
    
    //void Start()
    //{
        
    //}

    
    //void Update()
    //{
        
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objective.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objective.gameObject.SetActive(false);
        }
    }
}
