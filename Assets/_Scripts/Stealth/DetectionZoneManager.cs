using UnityEngine;
using System.Collections;

public class DetectionZoneManager : MonoBehaviour
{

    [SerializeField] private float timeDetectionZoneLasts;
    [SerializeField] private float timer;
    private bool timerActive;
    [SerializeField] private float timerStartValue;
    [SerializeField] private GameObject detectionZone;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = timerStartValue;
        timerActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && timerActive)
        {
            timerActive = false;
            detectionZone.SetActive(true);
            StartCoroutine(DetectionZoneTimer());
        }
    }


    private IEnumerator DetectionZoneTimer()
    {
        Debug.Log("Inicia countdown");
        yield return new WaitForSeconds(timeDetectionZoneLasts);
        Debug.Log("Fin countdown 1");
        detectionZone.SetActive(false);
        timer = timerStartValue;
        timerActive = true;
    }
}
