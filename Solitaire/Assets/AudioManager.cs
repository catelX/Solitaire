using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clips = new();
    public AudioSource source;

    public static AudioManager instance;

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

    public void PlayCardPickedClip()
    {
        source.clip = clips[0];
        source.Play();
    }
    public void PlayCardReleasedClip()
    {
        source.clip = clips[1];
        source.Play();
    }
    public void PlayCardShuffleClip()
    {
        source.clip = clips[2];
        source.Play();
    }
    public void PlayCardUndoClip()
    {
        source.clip = clips[3];
        source.Play();
    }

}
