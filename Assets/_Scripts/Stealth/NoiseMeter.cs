using UnityEngine;
using UnityEngine.UI;

public class NoiseMeter : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Image noiseMeterImage;
    [SerializeField] private Gradient noiseMeterColor;
    [SerializeField] private Image noiseMeterFrame;
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
        noiseMeterImage.fillAmount = amountOfNoise / maxNoiseUntilDefeat;
        // puse esto aca para poder comprobar que la barra ande mas facilmente
        noiseMeterImage.color = noiseMeterColor.Evaluate(amountOfNoise / maxNoiseUntilDefeat);
        noiseMeterFrame.color = noiseMeterColor.Evaluate(amountOfNoise / maxNoiseUntilDefeat);
        // cuando el sistema de obstaculos ande bien lo sacamos
    }
    public void IncreaseNoiseMeter()
    {
        AmountOfNoise++;
        noiseMeterImage.fillAmount = amountOfNoise / maxNoiseUntilDefeat;
        noiseMeterImage.color = noiseMeterColor.Evaluate(amountOfNoise / maxNoiseUntilDefeat);
        noiseMeterFrame.color = noiseMeterColor.Evaluate(amountOfNoise / maxNoiseUntilDefeat);
        if (AmountOfNoise == maxNoiseUntilDefeat)
        {
            gameManager.PlayerDetected = true;
        }
    }

    public void ResetNoise()
    {
        AmountOfNoise = 0f;
    }
}
