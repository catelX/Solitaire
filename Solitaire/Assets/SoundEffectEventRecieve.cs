using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectEventRecieve : MonoBehaviour
{

    public AudioSource soundEffect;

    void Start()
    {
        AudioManager.instance.onSoundEffectVolumeChange += ChangeSoundEffectVolume;
    }

    private void ChangeSoundEffectVolume(float _value)
    {
        soundEffect.volume = _value;
    }
}
