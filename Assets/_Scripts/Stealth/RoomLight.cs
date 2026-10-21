using UnityEngine;

// Ceiling light of a room. Knows how to turn itself on and off
public class RoomLight : MonoBehaviour
{
    [SerializeField] private Light roomLight;

    private void Start()
    {
        // Turn the light off again every time the player respawns
        GameManager.Instance.OnPlayerRespawn += TurnOff;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerRespawn -= TurnOff;
        }
            
    }

    public void TurnOn()
    {
        roomLight.enabled = true;
    }

    public void TurnOff()
    {
        roomLight.enabled = false;
    }
}