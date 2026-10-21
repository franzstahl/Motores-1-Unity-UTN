using System.Collections;
using UnityEngine;

public class CaughtSequence : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstepsClip;
    [SerializeField] private AudioClip voiceClip; // Optional

    [Header("Timing")]
    [SerializeField] private float footstepsDuration;
    [SerializeField] private float pauseAfterLight;
    [SerializeField] private float pauseAfterVoice; // Optional

    public IEnumerator Play()
    {
        // 1) Footsteps getting closer: volume increase over time

        if (footstepsClip != null && audioSource != null)
        { 
            audioSource.clip = footstepsClip;
            audioSource.loop = true;
            audioSource.volume = 0f;
            audioSource.Play();

            float t = 0f;

            // Gradually increase the volume of the footsteps over the specified duration

            while (t < footstepsDuration)
            {
                t += Time.deltaTime;
                audioSource.volume = Mathf.Clamp01(t / footstepsDuration);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(footstepsDuration); 
        }

        // 2) Turn on the ceiling light of the room the player is in

        if (GameManager.Instance.CurrentRoomLight != null)
        {
            GameManager.Instance.CurrentRoomLight.TurnOn();
        }

        yield return new WaitForSeconds(pauseAfterLight);

        // 3) Voice line (optional)

        if (voiceClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(voiceClip);
        }

        yield return new WaitForSeconds(pauseAfterVoice);

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

}
