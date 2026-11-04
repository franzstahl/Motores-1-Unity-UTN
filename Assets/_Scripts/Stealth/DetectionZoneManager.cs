using UnityEngine;
using System.Collections;

public class DetectionZoneManager : MonoBehaviour
{

    [SerializeField] private float timeDetectionZoneLasts;
    [SerializeField] private float timer;
    [SerializeField] private float timerStartValue;
    [SerializeField] private GameObject detectionZone;
    [SerializeField] private PickRandomClipFromTwoSets clipSelector;
    [SerializeField] private PickRandomClipFromTwoSets catClipSelector;
    public Animator anim;

    private bool timerActive;

    private void Start()
    {
        timer = timerStartValue;
        timerActive = true;
    }

    
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && timerActive)
        {
            timerActive = false;
            detectionZone.SetActive(false);
            anim.SetTrigger("PetCat");
            clipSelector.useVariantSet = true; //Variant set = petting
            catClipSelector.useVariantSet = true;
            StartCoroutine(DetectionZoneTimer());
        }
    }


    private IEnumerator DetectionZoneTimer()
    {
        Debug.Log("Inicia countdown");
        yield return new WaitForSeconds(timeDetectionZoneLasts);
        Debug.Log("Fin countdown 1");
        detectionZone.SetActive(true);
        clipSelector.useVariantSet = false;
        catClipSelector.useVariantSet = false;
        timer = timerStartValue;
        timerActive = true;
    }
}
