using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettingMenu()
    {

    }

    public void OpenMainMenu()
    {

    }

    public void OpenMainMenuScene()
    {

    }
}
