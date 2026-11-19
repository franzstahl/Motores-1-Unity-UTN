using System;
using TMPro;
using Unity.VisualScripting;
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
    private int minutes;
    private int seconds;
    private int miliseconds;
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

            minutes = Mathf.FloorToInt(countdownTimer / 60f); // calculates minutes, seconds and miliseconds
            seconds = Mathf.FloorToInt(countdownTimer % 60f);
            miliseconds = Mathf.FloorToInt((countdownTimer * 100f) % 100f);

            countdownTimerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, miliseconds); // sets de format

            if (countdownTimer <= 0)
            {
                GameManager.Instance.PlayerDetected = true;
                timerActive = false; // stops the process
                minutes = 0; // sets the timer to 0
                seconds = 0;
                miliseconds = 0;
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
