using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundVolumeController : MonoBehaviour
{
    [SerializeField] private string _key;
    private AudioSource _audioSource;

    private void Start()
    {   
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = PlayerPrefs.GetFloat(_key);
    }

    public void SetVolume(float volume)
    {
        _audioSource.volume = PlayerPrefs.GetFloat(_key);
        //_audioSource.volume = volume;
    }
}
