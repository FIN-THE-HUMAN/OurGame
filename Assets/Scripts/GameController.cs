using StarterAssets;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private int _mainSceneIndex;

    [SerializeField] private AudioClip _enemyDeath;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private int _enemyRemains;

    public UnityEvent<GameObject> OnEnemyDied;
    public UnityEvent OnExploded;
    public UnityEvent EnemyRemaines;

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
        PlaySound(enemy.transform.position, _enemyDeath);
        OnEnemyDied.Invoke(enemy);
        Destroy(enemy);

        DecreaseRemaineEnemies();
    }

    public void ListenEnemyDied(EnemyAI enemy)
    {

        if(enemy.TryGetComponent(out Health health))
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
            explosive.OnExplode.AddListener((e) => PlaySound(explosive.transform.position, _explosion));
        }
    }

    public void ListenEnemyDied(Health enemy)
    {
        enemy.OnDied.AddListener((e) => EnemyDied(enemy.gameObject));
    }

    public void ListenEnemyDied(EnemyAI enemy)
    {

        if(enemy.TryGetComponent(out Health health))
        {
            health.OnDied.AddListener((e) => EnemyDied(enemy.gameObject));
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

    public void PlaySound(Vector3 position, AudioClip _sound)
    {
        var sound = new GameObject("Sound");
        sound.transform.position = position;
        var audioSource = sound.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 25;
        audioSource.PlayOneShot(_sound);
        Destroy(sound, 2);
    }

    public void PlayExplosionSound(Explosive explosive)
    {
        PlaySound(explosive.transform.position, _explosion);
    }
}
