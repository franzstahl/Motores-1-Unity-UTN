using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class NoiseMeter : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Image noiseMeterImage;
    [SerializeField] private float maxNoiseUntilDefeat;
    [SerializeField] private float amountOfNoise;
    public float AmountOfNoise
    {
        get { return amountOfNoise; }
        set { amountOfNoise = Mathf.Min(value, maxNoiseUntilDefeat); }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        noiseMeterImage.fillAmount = AmountOfNoise/maxNoiseUntilDefeat;
    }

    // Update is called once per frame
    void Update()
    {
        noiseMeterImage.fillAmount = AmountOfNoise/maxNoiseUntilDefeat;
    }
    public void IncreaseNoiseMeter()
    {
        AmountOfNoise++;
        noiseMeterImage.fillAmount = AmountOfNoise/maxNoiseUntilDefeat;
        if(AmountOfNoise == maxNoiseUntilDefeat)
        {
            gameManager.PlayerDetected = true;
        }
    }
}
