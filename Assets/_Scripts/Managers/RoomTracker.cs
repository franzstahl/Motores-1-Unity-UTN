using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] private int currentRoom = 1;
    public int CurrentRoom => currentRoom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("RoomZone"))
        {
            RoomZone roomZone = other.gameObject.GetComponent<RoomZone>();
            currentRoom = roomZone.RoomNumber;
        }
    }
}
