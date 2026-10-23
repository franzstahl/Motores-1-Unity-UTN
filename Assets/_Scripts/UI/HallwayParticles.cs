using System;
using UnityEngine;

public class HallwayParticles : MonoBehaviour
{
    [Header("Particulas")]
    [SerializeField] private GameObject objective1;
    [SerializeField] private GameObject objective2;
    private int currentObjective;
    [Header("Componente en Player")]
    [SerializeField] private RoomTracker roomTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roomTracker.CurrentRoom >= 3 && roomTracker.CurrentRoom != 0)
        {
            currentObjective = 2;
        }
        if(roomTracker.CurrentRoom <= 2 && roomTracker.CurrentRoom != 0)
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
            if(currentObjective == 1)
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
