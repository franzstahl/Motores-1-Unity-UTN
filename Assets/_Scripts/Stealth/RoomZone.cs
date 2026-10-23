using UnityEngine;

// Trigger that covers a room. Tells the GameManager which light belongs to the room the player is in

[RequireComponent(typeof(Collider))]
public class RoomZone : MonoBehaviour
{
    [SerializeField] private int roomNumber;
    public int RoomNumber => roomNumber;
    [SerializeField] private RoomLight roomLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            GameManager.Instance.SetCurrentRoomLight(roomLight);
        }
            
    }
}