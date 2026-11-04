using UnityEngine;

public class RandomizeAudioSourceValues : MonoBehaviour
{
    [Header("Pitch range variation (multiplied by 1000)")]
    [SerializeField] private int minPitch;
    [SerializeField] private int maxPitch;

    [Header("Volume variation (multiplied by 1000)")]
    [SerializeField] private int minVolume;
    [SerializeField] private int maxVolume;

    private System.Random pitchRandomizer = new System.Random(); // Time
    private System.Random volumeRandomizer = new System.Random(); // Audio clips
    public void ChangeValues(AudioSource audioSource)
    {
        audioSource.pitch = pitchRandomizer.Next(minPitch, maxPitch)/1000.0f;
        audioSource.volume = volumeRandomizer.Next(minVolume, maxVolume) / 1000.0f;
    }
}
