using UnityEngine;

public class SpawnAndDespawnCat : MonoBehaviour
{
    [SerializeField] private GameObject catToSpawn;
    [SerializeField] private GameObject catToDespawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catToSpawn.SetActive(true);
            catToDespawn.SetActive(false);
        }
    }
}
