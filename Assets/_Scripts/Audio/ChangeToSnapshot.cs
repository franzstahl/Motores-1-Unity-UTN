
using UnityEngine;
using UnityEngine.Audio;

public class ChangeToSnapshot : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot snapshotToChangeInto;
    [SerializeField] private float changeTimer = 1f;

    [SerializeField] private AudioMixerSnapshot snapshotNormal;
    [SerializeField] private float tiempoRegreso = 1.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipToChangeInto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Cambio snap");

            if (snapshotToChangeInto != null)
            {
                snapshotToChangeInto.TransitionTo(changeTimer);
            }

            if (audioSource != null && clipToChangeInto != null)
            {
                audioSource.clip = clipToChangeInto;
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && snapshotNormal != null)
        {
            snapshotNormal.TransitionTo(tiempoRegreso);

            Debug.Log("Regreso a musica normal");
        }
    }
}
