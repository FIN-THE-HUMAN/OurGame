using TMPro;
using UnityEngine;

public class FullScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text fullScreenText;

    private void Awake()
    {
        fullScreenText.text = Screen.fullScreen ? "ON" : "OFF";
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;

        fullScreenText.text = Screen.fullScreen ? "ON" : "OFF";
    }
}
