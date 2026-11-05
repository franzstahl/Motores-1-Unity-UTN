using System;
using TMPro;
using UnityEngine;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timerActive = true;
            finalSequenceAudioParent.SetActive(true);
            scratchDoorParent.SetActive(true);
            variantMusicAudioParent.SetActive(false);
            countdownTimerTextParent.SetActive(true);
            catScratchingDoor.Play();
        }
    }
}
