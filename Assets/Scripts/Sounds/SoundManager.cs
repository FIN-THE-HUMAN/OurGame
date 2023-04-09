using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _enemyDeath;

    public void PlaySound()
    {
        var sound = new GameObject("Sound");
        var audioSource = sound.AddComponent<AudioSource>();
        audioSource.PlayOneShot(_enemyDeath);
    }

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
