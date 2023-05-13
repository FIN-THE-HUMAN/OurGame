using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class OpenedLevelsScriptableObject : ScriptableObject
{
    [SerializeField] private int _openedLevel = 1;

    public int OpenedLevel => _openedLevel; 

    public void OpenTempLevel()
    {
        if (_openedLevel <= SceneManager.GetActiveScene().buildIndex)
        {
            _openedLevel++;
        }
    }
}
