using UnityEngine;

public class KeySpawners : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    private void Start()
    {
        // Spawn the key at a random child position when the game starts
        if (keyPrefab == null || transform.childCount == 0) return;

        Transform point = transform.GetChild(Random.Range(0, transform.childCount));
        Instantiate(keyPrefab, point.position, point.rotation);
    }

}
