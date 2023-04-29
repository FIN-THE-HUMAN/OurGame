using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class SceneControllerScriptableObject : ScriptableObject
{
    public void OpenScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenMenu(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
