using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;

    public bool CanPlay { get; set; } = true;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayIfCanPlay()
    {
        if (CanPlay) _audioSource.Play();
    }
}
