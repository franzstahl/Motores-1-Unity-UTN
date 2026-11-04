using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class Key : MonoBehaviour, InteractibleInterface
{
    private SFXPlayer sfx2D;
    [SerializeField] private float groundOffset = 0.02f;
    private bool collected;
    private AudioSource audioSource;
    private RandomizeAudioSourceValues randVals;

    [Header("Post Processing Pulse")]
    [SerializeField] private Volume pickUpVolume;
    [SerializeField, Range(0f, 1f)] private float peakWeight = 1f;
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float holdDuration = 0.15f;
    [SerializeField] private float fadeOutDuration = 0.8f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        randVals = GetComponent<RandomizeAudioSourceValues>();

        // Si la llave es un prefab instanciado, no se puede asignar un Volume de escena
        // desde el inspector, así que lo buscamos por tag (ver nota abajo).
        if (pickUpVolume == null)
        {
            GameObject volObj = GameObject.FindWithTag("KeyPickUpVolume");
            if (volObj != null) pickUpVolume = volObj.GetComponent<Volume>();
        }
        if (pickUpVolume != null) pickUpVolume.weight = 0f;

        StartCoroutine(CicloDeMusica());
        SnapToSurface();
    }

    private void SnapToSurface()
    {
        Renderer r = GetComponentInChildren<Renderer>();
        if (r == null) return;

        Vector3 origin = transform.position + Vector3.up * 1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 3f, ~0, QueryTriggerInteraction.Ignore))
        {
            float bottomOffset = transform.position.y - r.bounds.min.y;
            Vector3 pos = transform.position;
            pos.y = hit.point.y + bottomOffset + groundOffset;
            transform.position = pos;
        }
    }

    public void SetSFX(SFXPlayer player)
    {
        sfx2D = player;
    }

    public void Interact()
    {
        if (collected) return;
        collected = true;

        if (sfx2D != null) sfx2D.Play("KeyPickUp");
        GameManager.Instance.CollectKey();

        // Apagamos todo lo visible/interactivo, pero dejamos vivo el objeto para el pulso
        StopAllCoroutines();
        if (audioSource != null) audioSource.Stop();
        foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = false;

        if (pickUpVolume != null)
            StartCoroutine(PulseVolumeAndDestroy());
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }

    private IEnumerator PulseVolumeAndDestroy()
    {
        float startWeight = pickUpVolume.weight;

        // 1) Sube rápido
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            pickUpVolume.weight = Mathf.Lerp(startWeight, peakWeight, t / fadeInDuration);
            yield return null;
        }
        pickUpVolume.weight = peakWeight;

        // 2) Mantiene el pico
        yield return new WaitForSeconds(holdDuration);

        // 3) Baja suavemente
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            pickUpVolume.weight = Mathf.Lerp(peakWeight, 0f, t / fadeOutDuration);
            yield return null;
        }
        pickUpVolume.weight = 0f;

        Destroy(gameObject);
    }

    private IEnumerator CicloDeMusica()
    {
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            randVals.ChangeValues(audioSource);
        }
    }
}