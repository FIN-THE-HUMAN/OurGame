using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _LoseMenuUI;
    [SerializeField] private List<GameObject> _disactivable;
    [SerializeField] private FirstPersonController _playerController;
    [SerializeField] private List<MonoBehaviour> _disenabable;
    [SerializeField] private List<Scrollable> _scrollables;
    public bool _canOpenPauseMenu = true;

    private bool _paused = false;
    private bool _lose = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetCanOpenPauseMenu(bool canOpenPauseMenu)
    {
        _canOpenPauseMenu = canOpenPauseMenu;
    }

>>>>>>> Stashed changes
    public void ActivateMenuComponent(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

<<<<<<< Updated upstream
=======
    public void Win()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lose()
    {
        _lose = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _playerController.enabled = false;
        _LoseMenuUI.SetActive(true);
        _disactivable.ForEach(x => x.SetActive(false));
        _disenabable.ForEach(x => x.enabled = false);
        _scrollables.ForEach(e => e.Active = false);
        Time.timeScale = 0;
        //_paused = true;
    }

>>>>>>> Stashed changes
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

    public void Pause()
    {
        if (!_canOpenPauseMenu) return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _playerController.enabled= false;
        _pauseMenuUI.SetActive(true);
        _disactivable.ForEach(x => x.SetActive(false));
        _disenabable.ForEach(x => x.enabled = false);
        _scrollables.ForEach(e => e.Active = false);
        Time.timeScale = 0;
        _paused = true;
    }

    public void UnPause()
    {
        Cursor.visible = false;
        _playerController.enabled= true;
        _pauseMenuUI.SetActive(false);
        _disactivable.ForEach(x => x.SetActive(true));
        _disenabable.ForEach(x => x.enabled = true);
        _scrollables.ForEach(e => e.Active = true);
        Time.timeScale = 1;
        _paused = false;
    }

    private void Update()
    {
        if (_lose)
        {
            return;
        }

        if (Input.GetKeyDown(_pauseKey))
        {
            if (_paused)
                UnPause();
            else 
                Pause();
        }
    }
}
