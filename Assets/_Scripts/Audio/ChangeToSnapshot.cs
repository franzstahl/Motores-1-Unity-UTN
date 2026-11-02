using UnityEngine;
using UnityEngine.Audio;

public class ChangeToSnapshot : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot snapshotToChangeInto;
    [SerializeField] private float changeTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            snapshotToChangeInto.TransitionTo(changeTimer);
        }
    }
}
