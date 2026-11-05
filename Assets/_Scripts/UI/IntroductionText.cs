using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class IntroductionText : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private List<GameObject> introductionTexts = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (startButton != null)
        //{
        //    startButton.onClick.AddListener(StartIntroduction);
        //}
       
    }
    public void StartIntroduction()
    {
        StartCoroutine(Introduction());
    }

    private IEnumerator Introduction()
    {
        yield return new WaitForSeconds(1);
        panel.gameObject.SetActive(true);
        introductionTexts[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        introductionTexts[0].gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        introductionTexts[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        introductionTexts[1].gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        introductionTexts[2].gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        introductionTexts[2].gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        introductionTexts[3].gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        introductionTexts[3].gameObject.SetActive(false);

        panel.gameObject.SetActive(false);
    }

}
