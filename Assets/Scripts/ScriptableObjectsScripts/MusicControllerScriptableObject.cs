using UnityEngine;

[CreateAssetMenu()]
public class MusicControllerScriptableObject : ScriptableObject
{
    [SerializeField] private AudioSource _audioSource;

    public void MusicSlider(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        Debug.Log(PlayerPrefs.GetFloat("MusicVolume"));
    }

    //public void Start()
    //{
    //    _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    //}

    //public void UpdateVolume()
    //{
    //    _audioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
    //}
}
