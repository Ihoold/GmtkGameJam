using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public SoundSO backgroundMusic;
    AudioSource backgroundMusicSource;
    

    void Start()
    {
        backgroundMusicSource = backgroundMusic.AddAudioSource(this.gameObject);
        backgroundMusicSource.Play();
    }
}
