using UnityEngine;
using UnityEngine.Audio;

public class ChangeToSnapshot : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot snapshotToChangeInto;
    [SerializeField] private float changeTimer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipToChangeInto;
    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Cambio snap");
            snapshotToChangeInto.TransitionTo(changeTimer);
            if (audioSource != null && clipToChangeInto != null)
            {
                audioSource.clip = clipToChangeInto;
                audioSource.PlayScheduled(Time.deltaTime + 1f);
            }
        }
    }
}
