using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Start()
    {
        // Load saved volume from PlayerPrefs
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float savedSoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0f);

        // Set the saved volume
        SetMusicVolume(savedMusicVolume);
        SetSoundVolume(savedSoundVolume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
        PlayerPrefs.SetFloat("SoundVolume", volume);
    }
}