using UnityEngine;

public class UIsounds : MonoBehaviour
{
    // Singleton for a sound Manager
    static UIsounds instance;
    public static UIsounds Instance => instance;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip soundClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
   
    private void Start()
    {
        
    }

    
    public void PlaySound()
    {
        source.PlayOneShot(soundClip);
    }
}
