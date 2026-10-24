using System;
using UnityEngine;

public class HallwayParticles : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private GameObject objective1;
    [SerializeField] private GameObject objective2;
  
    [Header("Component in Player")]
    [SerializeField] private RoomTracker roomTracker;

    private int currentObjective;

    void Awake()
    {
        roomTracker = FindFirstObjectByType<RoomTracker>();
    }

    private void Update()
    {
        if (roomTracker.CurrentRoom >= 3 && roomTracker.CurrentRoom != 0)
        {
            currentObjective = 2;
        }
        if (roomTracker.CurrentRoom <= 2 && roomTracker.CurrentRoom != 0)
        {
            currentObjective = 1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentObjective == 1)
            {
                objective1.gameObject.SetActive(true);
            }
            else
            {
                objective2.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentObjective == 1)
            {
                objective1.gameObject.SetActive(false);
            }
            else
            {
                objective2.gameObject.SetActive(false);
            }
            
        }
    }
}
