using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ActivateMenuComponent(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisactivateMenuComponent(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

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
