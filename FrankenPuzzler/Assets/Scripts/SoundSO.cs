using UnityEngine;


[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/Sound")]
public class SoundSO : ScriptableObject
{
    public string soundName;

    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    public AudioSource AddAudioSource(GameObject sourceObject) {
        AudioSource source = sourceObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        return source;
    }
}
