using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXEntry
{
    public string id;
    public AudioClip[] clips;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    public Vector2 volumeRange = new Vector2(0.85f, 1f);
}

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private SFXEntry[] sfx;

    private AudioSource source;
    private Dictionary<string, int> lastIndex = new Dictionary<string, int>();

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(string id)
    {
        foreach (SFXEntry entry in sfx)
        {
            if (entry.id != id) continue;
            if (entry.clips.Length == 0) return;

            int index = Random.Range(0, entry.clips.Length);
            if (entry.clips.Length > 1 && lastIndex.TryGetValue(id, out int last) && index == last)
            {
                index = (index + 1) % entry.clips.Length; // No repite el mismo clip dos veces seguidas
            }
             
            lastIndex[id] = index;

            source.pitch = Random.Range(entry.pitchRange.x, entry.pitchRange.y);
            source.PlayOneShot(entry.clips[index], Random.Range(entry.volumeRange.x, entry.volumeRange.y));
            return;
        }

        Debug.LogWarning($"SFXPlayer: no existe el sonido '{id}' en {name}");
    }
}