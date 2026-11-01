using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class KitchenTimer : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float limit;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;
    [SerializeField] private int miliseconds;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private bool startCountdown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = limit;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCountdown == true)
        {
            timer -= Time.deltaTime;
 
            minutes = Mathf.FloorToInt(timer / 60f); // calculates minutes, seconds and miliseconds
            seconds = Mathf.FloorToInt(timer % 60f);
            miliseconds = Mathf.FloorToInt((timer * 100f) % 100f);

            number.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, miliseconds); // sets de format

            if (timer <=0)
            {
                GameManager.Instance.PlayerDetected = true;
                startCountdown = false; // stops the process
                minutes = 0; // sets the timer to 0
                seconds = 0;
                miliseconds = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other) // kitchen zone trigger
    {
        if (other.gameObject.CompareTag("Player"))
        {
            startCountdown = true;
        }
    }
}
