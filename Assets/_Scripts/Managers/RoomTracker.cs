using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [SerializeField] private int currentRoom = 1;
    public int CurrentRoom => currentRoom;
   
    //void Start()
    //{
        
    //}

    
    //void Update()
    //{
        
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RoomZone"))
        {
            RoomZone roomZone = other.gameObject.GetComponent<RoomZone>();
            currentRoom = roomZone.RoomNumber;
        }
    }
}
