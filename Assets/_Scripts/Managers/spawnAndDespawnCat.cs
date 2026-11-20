using UnityEngine;

public class SpawnAndDespawnCat : MonoBehaviour
{
    [SerializeField] private GameObject catToSpawn;
    [SerializeField] private GameObject catToDespawn;
    [SerializeField] private SFXPlayer sfx2D;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (catToSpawn != null && !catToSpawn.activeSelf)
            {
                catToSpawn.SetActive(true);

                if (sfx2D != null)
                    sfx2D.Play("CatAppears");
            }

            if (catToDespawn != null)
                catToDespawn.SetActive(false);
        }
    }
}
