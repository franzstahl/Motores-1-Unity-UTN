using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isMovementActive;
    private bool resetTimerActive;
    [SerializeField] private float resetTimer;
    private float resetTimerOriginalValue;
    private bool playerDetected;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 5.0f;
    public GameObject player;

    public AudioSource audioSource;
    public AudioClip clip;

    private Coroutine fadeCoroutine;

    void Start()
    {
        player = GameObject.Find("Player");
        resetTimerActive = false;
        resetTimerOriginalValue = resetTimer;
        EnterPlayingState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            playerDetected = true;

        if (Input.GetKeyDown(KeyCode.G))
            FadeOut();

        if (Input.GetKeyDown(KeyCode.H))
            FadeIn();

        if (playerDetected)
        {
            playerDetected = false;
            EnterDetectedState();
        }

        if (resetTimerActive)
            resetTimer -= Time.deltaTime;

        if (resetTimer < 0 && resetTimerActive)
            EnterRestartingState();
    }

    public void EnterPlayingState()
    {
        FadeIn();
        isMovementActive = true;
        playerDetected = false;
        resetTimerActive = false;
        resetTimer = resetTimerOriginalValue;
    }

    public void EnterDetectedState()
    {
        Debug.Log("Jugador detectado");
        audioSource.PlayOneShot(clip);
        isMovementActive = false;
        resetTimerActive = true;
    }

    public void EnterRestartingState()
    {
        resetTimerActive = false; // stop timer immediately so this can't re-trigger
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        // Wait for the screen to fully fade to black before moving the player
        yield return StartFade(1f);

        player.transform.position = startingPosition;

        EnterPlayingState(); // fades back in
    }

    // Opacity control

    public void FadeIn() => StartFade(0f);
    public void FadeOut() => StartFade(1f);

    private Coroutine StartFade(float target)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(canvasGroup.alpha, target, fadeDuration));
        return fadeCoroutine;
    }

    private IEnumerator FadeCanvasGroup(float start, float end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = end;
        fadeCoroutine = null;
    }
}