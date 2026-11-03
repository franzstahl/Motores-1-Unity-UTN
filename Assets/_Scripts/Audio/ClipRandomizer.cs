using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ClipRandomizer : MonoBehaviour
{
    [Header ("Audio Clips")]
    [SerializeField] private List<AudioClip> Clips = new List<AudioClip>();

    [Header ("Time between clips")]
    [SerializeField] private int minInterval;
    [SerializeField] private int maxInterval;

    [SerializeField] public bool modifyAudioSource;
    [SerializeField] public RandomizeAudioSourceValues randomizeAudioSource;
    

    [Header ("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    private int listIndex;
    private int interval;
    private float timer;
    private System.Random intervalRandomizer = new System.Random(); // Time
    private System.Random clipRandomizer = new System.Random(); // Audio clips

    
    private void Start()
    {
        
    }

   
    private void Update()
    {
        interval = intervalRandomizer.Next(minInterval, maxInterval);
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            if (modifyAudioSource) randomizeAudioSource.ChangeValues(audioSource);
            listIndex = clipRandomizer.Next(0, Clips.Count);
            audioSource.clip = Clips[listIndex];
            audioSource.Play();
            timer = 0;
        }
    }
}
