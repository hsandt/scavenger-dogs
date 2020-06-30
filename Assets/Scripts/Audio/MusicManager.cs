using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CommonsHelper;
using CommonsPattern;

public class MusicManager : SingletonManager<MusicManager> {

    protected MusicManager () {} // guarantee this will be always a singleton only - can't use the constructor!

    /* Sibling components */
    private AudioSource audioSource;
    
    /* State */
    
    /// Was the BGM playing (before pause)?
    /// Helps to resume playing after a pause
    private bool m_WasAudioSourcePlaying = false;

    protected override void Init()
    {
        base.Init();
        
        audioSource = this.GetComponentOrFail<AudioSource>();
    }

    public void Play () {
        audioSource.Play();
    }

    void OnEnable () {
        enabled = true;
        if (m_WasAudioSourcePlaying) {
            audioSource.Play();
            m_WasAudioSourcePlaying = false;
        }
    }

    void OnDisable () {
        if (audioSource.isPlaying) {
            audioSource.Pause();
            m_WasAudioSourcePlaying = true;
        }
    }

}
