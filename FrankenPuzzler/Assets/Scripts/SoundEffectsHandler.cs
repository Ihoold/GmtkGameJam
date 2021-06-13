using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsHandler : MonoBehaviour
{
    Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
    SoundSO[] soundEffects;

    void Start() {
        foreach (SoundSO effect in soundEffects) {
            sources[effect.soundName] = effect.AddAudioSource(this.gameObject);
        }
    }

    public void PlaySoundEffect(string effectName) {
        sources[effectName].Play();
    }
}
