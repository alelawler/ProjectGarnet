using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHelper : MonoBehaviour
{
    SoundHelper myInstance;

    [SerializeField]
    private AudioSource myAudioSource;

    private void Awake()
    {
        myInstance = this;
    }
    public void PlaySound(AudioClip clipToPlay, float volume = 0.5f, float pitch = 1f)
    {
        if (myInstance != null && clipToPlay != null)
        {
            myInstance.myAudioSource.pitch = pitch;
            myInstance.myAudioSource.PlayOneShot(clipToPlay, volume);
        }
            
    }



}
