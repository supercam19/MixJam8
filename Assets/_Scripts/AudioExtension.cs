using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExtension : MonoBehaviour
{
    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayVaried(AudioClip clip, float variance)
    {
        audioSource.pitch = Random.Range(1 - variance, 1 + variance);
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayRandom(AudioClip[] clips)
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
    
    public void PlayRandomVaried(AudioClip[] clips, float variance)
    {
        audioSource.pitch = Random.Range(1 - variance, 1 + variance);
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
