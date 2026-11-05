using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartCountdown : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownTimerText;
    [SerializeField] private GameObject countdownTimerTextParent;
    [SerializeField] private GameObject finalSequenceAudioParent;
    [SerializeField] private GameObject scratchDoorParent;
    [SerializeField] private AudioSource catScratchingDoor;
    [SerializeField] private GameObject variantMusicAudioParent;
    [SerializeField] private float countdownTimer;
    [SerializeField] private bool timerActive;
    [SerializeField] private AudioSource finalSectionAudioSource;
    [SerializeField] private bool deactivator;
    private float timerReseter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerReseter = countdownTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            countdownTimer -= Time.deltaTime;
            countdownTimerText.text = countdownTimer.ToString();
            if (countdownTimer <= 0)
            {
                GameManager.Instance.PlayerDetected = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !deactivator)
        {
            timerActive = true;
            finalSequenceAudioParent.SetActive(true);
            scratchDoorParent.SetActive(true);
            variantMusicAudioParent.SetActive(false);
            countdownTimerTextParent.SetActive(true);
            catScratchingDoor.Play();
            finalSectionAudioSource.PlayScheduled(Time.deltaTime + 1f);
        }
        else if (other.tag == "Player" && deactivator)
        {
            timerActive = false;
            countdownTimer = timerReseter;
            finalSequenceAudioParent.SetActive(false);
            scratchDoorParent.SetActive(false);
            variantMusicAudioParent.SetActive(true);
            countdownTimerTextParent.SetActive(false);
            catScratchingDoor.Stop();
            finalSectionAudioSource.Stop();
        }
    }
}
