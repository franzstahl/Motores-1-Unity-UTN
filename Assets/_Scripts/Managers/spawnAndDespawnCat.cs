using UnityEngine;

public class SpawnAndDespawnCat : MonoBehaviour
{
    [SerializeField] private GameObject catToSpawn;
    [SerializeField] private GameObject catToDespawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(catToSpawn != null) catToSpawn.SetActive(true);
            if (catToDespawn != null) catToDespawn.SetActive(false);
        }
    }
}
