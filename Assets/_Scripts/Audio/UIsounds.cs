using UnityEngine;
using System.Collections.Generic;

public class UIsounds : MonoBehaviour
{
    // Singleton for a sound Manager
    static UIsounds instance;
    public static UIsounds Instance => instance;
    [Header("Audio Source")]
    [SerializeField] private AudioSource source;
    [Header ("Audio Clips")]
    [SerializeField] private List<AudioClip> soundClips = new List<AudioClip>();
    private int listIndex;
    private System.Random clipRandomizer = new System.Random();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
   
    private void Start()
    {
        
    }

    
    public void PlaySound()
    {
        listIndex = clipRandomizer.Next(0, soundClips.Count);
        source.PlayOneShot(soundClips[listIndex]);
    }
}
