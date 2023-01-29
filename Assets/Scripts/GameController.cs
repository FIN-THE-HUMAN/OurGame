using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private int _mainSceneIndex;

    public UnityEvent<GameObject> OnEnemyDied;

    public void PlayerDied(GameObject player)
    {
        var controller = player.GetComponent<FirstPersonController>();
        if (controller) controller.enabled = false;
        Time.timeScale = 0;
    }

    public void EnemyDied(GameObject enemy)
    {
        OnEnemyDied.Invoke(enemy);
        Destroy(enemy);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(_mainSceneIndex);
    }
}
