using System.Collections.Generic;
using UnityEngine;

public class PickRandomClipFromTwoSets : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> ClipSet1 = new List<AudioClip>();
    [SerializeField] private List<AudioClip> ClipSet2 = new List<AudioClip>();

    public bool useVariantSet;

    [SerializeField] public bool modifyAudioSource;
    [SerializeField] public RandomizeAudioSourceValues randomizeAudioSource;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    private int listIndex;
    [SerializeField] private int interval;
    private float timer;
    //private System.Random intervalRandomizer = new System.Random(); // Time
    private System.Random clipRandomizer = new System.Random(); // Audio clips


    private void Start()
    {

    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            if(modifyAudioSource) randomizeAudioSource.ChangeValues(audioSource);
            if (!useVariantSet)
            {
                listIndex = clipRandomizer.Next(0, ClipSet1.Count);
                audioSource.clip = ClipSet1[listIndex];
                audioSource.Play();
            }
            else
            {
                listIndex = clipRandomizer.Next(0, ClipSet2.Count);
                audioSource.clip = ClipSet2[listIndex];
                audioSource.Play();
            }
            timer = 0;
        }
    }
}
