using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class GameControllerScriptableObject : ScriptableObject
{
    [SerializeField] private int _mainSceneIndex;

    [SerializeField] private AudioClip _enemyDeathSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private int _enemyRemains;
    [SerializeField] private int _maxLevelIndex = 2;

    public UnityEvent<GameObject> OnEnemyDied;
    public UnityEvent OnExploded;
    public UnityEvent EnemyRemaines;

    public void Restart()
    {
        foreach (var controller in FindObjectsOfType<FirstPersonController>())
        {
            controller.enabled = true;
        }
        Time.timeScale = 1;
    }

    public void PlayerDied(GameObject player)
    {
        var controller = player.GetComponent<FirstPersonController>();
        if (controller) controller.enabled = false;
        Time.timeScale = 0;
    }

    public void DecreaseRemaineEnemies()
    {
        _enemyRemains--;
        if (_enemyRemains <= 0)
        {
            Debug.Log("EnemyRemaines <Game Ended>");
            EnemyRemaines.Invoke();
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        PlaySound(enemy.transform.position, _enemyDeathSound);
        OnEnemyDied.Invoke(enemy);
        Destroy(enemy);

        DecreaseRemaineEnemies();
    }

    public void ListenEnemyDied(EnemyAI enemy)
    {

        if (enemy.TryGetComponent(out Health health))
        {
            health.OnDied.AddListener((e) => EnemyDied(enemy.gameObject));
        }

    }

    public void ListenExplosionWeapon(Gun gun, Rigidbody rigidbody)
    {
        Explosive explosive;
        if (rigidbody.TryGetComponent(out explosive))
        {
            explosive.OnExplode.AddListener((e) => OnExploded.Invoke());
            explosive.OnExplode.AddListener((e) => PlaySound(explosive.transform.position, _explosionSound));
        }
    }

    public void ListenEnemyDied(Health enemy)
    {
        enemy.OnDied.AddListener((e) => EnemyDied(enemy.gameObject));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(_mainSceneIndex);
    }

    public void RestartLevel()
    {
        Restart();
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlaySound(Vector3 position, AudioClip _sound)
    {
        var sound = new GameObject("Sound");
        sound.transform.position = position;
        var audioSource = sound.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 25;
        audioSource.PlayOneShot(_sound);
        Destroy(sound, 2);
    }

    public void PlayExplosionSound(Explosive explosive)
    {
        PlaySound(explosive.transform.position, _explosionSound);
    }
}
