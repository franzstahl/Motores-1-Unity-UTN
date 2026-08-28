using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class GameManager : MonoBehaviour
{
    public bool isMovementActive;
    private bool resetTimerActive;
    [SerializeField] private float resetTimer;
    private float resetTimerOriginalValue;
    private bool playerDetected;
    [SerializeField] private Vector3 startingPosition;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        resetTimerActive = false;
        resetTimerOriginalValue = resetTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            playerDetected = true;
        if (playerDetected)
            EnterDetectedState();
        if (resetTimerActive)
        {
            resetTimer -= Time.deltaTime;
            //Debug.Log(resetTimer);
        }

        if (resetTimer < 0 && resetTimerActive)
        {
            EnterRestartingState();
        }
    }

    public void EnterPlayingState()
    {
        isMovementActive = true;
        playerDetected = false;
        resetTimerActive = false;
        resetTimer = resetTimerOriginalValue;
    }

    public void EnterDetectedState()
    {
        Debug.Log("Jugador detectado");
        playerDetected = true;
        isMovementActive = false;
        resetTimerActive = true;
    }

    public void EnterRestartingState()
    {
        player.transform.position = startingPosition;
        EnterPlayingState();
    }

    //1. Estados del juego: Playing, Detected, Restarting.
    //2. Función PlayerDetected(): se llama cuando te descubren.Cambia el estado, frena al jugador, tira un placeholder de luz/sonido(por ahora alcanza con un Debug.Log), y después de 1-2 segundos reinicia.
    //3. Función RestartGame(): recarga la escena entera desde cero con SceneManager.LoadScene.No hay checkpoints, siempre se vuelve al inicio.
    //4. Patrón Singleton: que solo exista una instancia del GameManager en la escena, accesible desde cualquier script con GameManager.Instance.
    //5. Se puede probar todo esto sin esperar a nadie más, simulando la detección con una tecla de testeo (por ejemplo, apretar T llama a PlayerDetected() a mano).
}
