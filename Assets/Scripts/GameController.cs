using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private int _mainSceneIndex;

    public void PlayerDied(GameObject player)
    {
        var controller = player.GetComponent<FirstPersonController>();
        if (controller) controller.enabled = false;
        Time.timeScale = 0;
    }

    public void EnemyDied(GameObject enemy)
    {
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
