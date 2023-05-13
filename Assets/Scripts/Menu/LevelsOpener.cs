using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsOpener : MonoBehaviour
{
    [SerializeField] private OpenedLevelsScriptableObject _openedLevels;
    [SerializeField] private GameObject _locked;

    private void Start()
    {
        var levelsIcons = transform.GetComponentsInChildren<Button>();

        for (int i = 0; i < _openedLevels.OpenedLevel; i++)
        {
            levelsIcons[i].interactable = true;
        }

        for (int i = _openedLevels.OpenedLevel; i < levelsIcons.Length; i++)
        {
            levelsIcons[i].interactable = false;
            var locked = Instantiate(_locked, levelsIcons[i].transform.position, Quaternion.identity);
            locked.transform.parent = levelsIcons[i].transform;
        }
    }
}
