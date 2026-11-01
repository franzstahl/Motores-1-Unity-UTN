using UnityEngine;

public class moveObjectToMe : MonoBehaviour
{
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.position = transform.position;
    }
}
