using UnityEngine;

// Ceiling light of a room. Knows how to turn itself on and off
public class RoomLight : MonoBehaviour
{
    [SerializeField] private Light[] roomLights;
    [SerializeField] private SFXPlayer sfx2D;
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
        if (sfx2D != null)
            sfx2D.Play("RoomLightsOn");


        foreach (Light light in roomLights)
        {
            light.enabled = true;
        }
    }

    public void TurnOff()
    {
        foreach (Light light in roomLights)
        {
            light.enabled = false;
        }
    }
}