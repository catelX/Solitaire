using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clips = new();
    public AudioSource soundEffects;
    public AudioSource BGM;

    public static AudioManager instance;

    public event Action<float> onSoundEffectVolumeChange;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeBGMVolume(float _value)
    {
        BGM.volume = _value;
    }

    public void ChangeSoundEffectsVolume(float _value)
    {
        onSoundEffectVolumeChange(_value);
    }

    public void PlayCardPickedClip()
    {
        soundEffects.clip = clips[0];
        soundEffects.Play();
    }
    public void PlayCardReleasedClip()
    {
        soundEffects.clip = clips[1];
        soundEffects.Play();
    }
    public void PlayCardShuffleClip()
    {
        soundEffects.clip = clips[2];
        soundEffects.Play();
    }
    public void PlayCardUndoClip()
    {
        soundEffects.clip = clips[3];
        soundEffects.Play();
    }

}
