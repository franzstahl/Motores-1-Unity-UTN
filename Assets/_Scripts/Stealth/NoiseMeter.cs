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
        private set { amountOfNoise = Mathf.Min(value, maxNoiseUntilDefeat); }
    }
   
    private void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        noiseMeterImage.fillAmount = AmountOfNoise/maxNoiseUntilDefeat;
    }

    
   private void Update()
    {
        noiseMeterImage.fillAmount = AmountOfNoise/maxNoiseUntilDefeat;
    }
    public void IncreaseNoiseMeter()
    {
        AmountOfNoise++;
        noiseMeterImage.fillAmount = AmountOfNoise/maxNoiseUntilDefeat;
        if (AmountOfNoise == maxNoiseUntilDefeat)
        {
            gameManager.PlayerDetected = true;
        }
    }
}
