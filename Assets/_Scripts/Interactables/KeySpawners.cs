using UnityEngine;

public class KeySpawners : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    private void Start()
    {
        if (keyPrefab == null || transform.childCount == 0) return;

        Transform point = transform.GetChild(Random.Range(0, transform.childCount));
        Instantiate(keyPrefab, point.position, point.rotation);
    }

}
