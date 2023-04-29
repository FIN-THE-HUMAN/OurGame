using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KillEnemiesTaskManager : TaskManager
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int enemies = 5;

    public void DecreaseEnemyNumbers()
    {
        enemies--;
        if (enemies > 0)
        {
            _text.text = $"Task: kill {enemies} enemies";
        }
        else
        {
            _text.text = "Task completed";
            OnTaskCompleted.Invoke();
        }
    }
}
