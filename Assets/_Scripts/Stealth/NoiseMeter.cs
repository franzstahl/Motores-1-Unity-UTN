using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class NoiseMeter : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Image noiseMeterImage;
    [SerializeField] private Gradient noiseMeterColor;
    [SerializeField] private Image noiseMeterFrame;
    [SerializeField] private float maxNoiseUntilDefeat;
    [SerializeField] private float amountOfNoise;

    [Header("Post Processing Pulse")]
    [SerializeField] private Volume madeNoiseVolume;
    [SerializeField, Range(0f, 1f)] private float peakWeight = 1f;   // intensidad máxima del efecto
    [SerializeField] private float fadeInDuration = 0.1f;            // qué tan rápido sube
    [SerializeField] private float holdDuration = 0.15f;             // cuánto se mantiene en el pico
    [SerializeField] private float fadeOutDuration = 0.8f;           // qué tan lento baja
    private Coroutine pulseCoroutine;

    public float AmountOfNoise
    {
        get { return amountOfNoise; }
        private set { amountOfNoise = Mathf.Min(value, maxNoiseUntilDefeat); }
    }

    private void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        madeNoiseVolume.weight = 0f; // arranca sin efecto
        UpdateMeterVisuals();
    }

    private void Update()
    {
        UpdateMeterVisuals();

        
        // cuando el sistema de obstaculos ande bien lo sacamos
    }

    private void UpdateMeterVisuals()
    {
        float normalized = amountOfNoise / maxNoiseUntilDefeat;
        noiseMeterImage.fillAmount = normalized;
        noiseMeterImage.color = noiseMeterColor.Evaluate(normalized);
        noiseMeterFrame.color = noiseMeterColor.Evaluate(normalized);
    }

    public void IncreaseNoiseMeter()
    {
        AmountOfNoise++;
        UpdateMeterVisuals();

        if (AmountOfNoise >= maxNoiseUntilDefeat)
        {
            gameManager.PlayerDetected = true;
        }

        StartPulse(); // ahora pulsa siempre que se hace ruido
    }

    public void ResetNoise()
    {
        AmountOfNoise = 0f;
    }

    private void StartPulse()
    {
        if (pulseCoroutine != null)
            StopCoroutine(pulseCoroutine);

        pulseCoroutine = StartCoroutine(PulseVolume());
    }

    private IEnumerator PulseVolume()
    {
        float startWeight = madeNoiseVolume.weight; // por si ya venía a mitad de un pulso

        // 1) Sube rápido
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            madeNoiseVolume.weight = Mathf.Lerp(startWeight, peakWeight, t / fadeInDuration);
            yield return null;
        }
        madeNoiseVolume.weight = peakWeight;

        // 2) Mantiene el pico un momento
        yield return new WaitForSeconds(holdDuration);

        // 3) Baja suavemente
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            madeNoiseVolume.weight = Mathf.Lerp(peakWeight, 0f, t / fadeOutDuration);
            yield return null;
        }
        madeNoiseVolume.weight = 0f;
        pulseCoroutine = null;
    }
}