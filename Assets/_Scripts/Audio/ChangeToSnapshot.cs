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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            snapshotToChangeInto.TransitionTo(changeTimer);
            if (audioSource != null && clipToChangeInto != null)
            {
                audioSource.clip = clipToChangeInto;
                audioSource.PlayScheduled(Time.deltaTime + 1f);
            }
        }
    }
}
