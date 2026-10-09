using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float resetTimer;
    [SerializeField] private bool playerDetected;
    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 5.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    public bool isMovementActive { get; private set; }
    private bool resetTimerActive;
    private float resetTimerOriginalValue;
    public GameObject player;
    private Coroutine fadeCoroutine;

    public bool PlayerDetected
    {
        get { return playerDetected; }
        set { playerDetected = value; }
    }

    public static GameManager Instance { get; private set; } // Anyone can read it, only this class can set it

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        player = GameObject.Find("Player");
        resetTimerActive = false;
        resetTimerOriginalValue = resetTimer;
        EnterPlayingState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            playerDetected = true;

        if (Input.GetKeyDown(KeyCode.G))
            FadeOut();

        if (Input.GetKeyDown(KeyCode.H))
            FadeIn();

        if (playerDetected)
        {
            EnterDetectedState();
            playerDetected = false;
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
        resetTimerActive = false; // Stop timer immediately so this can't re-trigger
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        // Wait for the screen to fully fade to black before moving the player
        yield return StartFade(1f);

        SceneManager.LoadScene("MainLevel 1");

        EnterPlayingState(); // Fades back in
    }

    // Opacity control

    public void FadeIn() => StartFade(0f);
    public void FadeOut() => StartFade(1f);

    private Coroutine StartFade(float target)
    {
        if (canvasGroup == null)
        {
            return null;
        }
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