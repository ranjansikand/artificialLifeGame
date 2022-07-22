using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    PlayerInput _playerInput;
    [SerializeField] GameObject _menu;

    void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Menu.ToggleMenu.performed += OnToggleMenu;
        _playerInput.Menu.Quit.performed += OnQuitPressed;
    }

    private void OnEnable() { _playerInput.Enable(); }
    private void OnDisable() { _playerInput.Disable(); }

    private void OnToggleMenu(InputAction.CallbackContext context) {
        if (_menu == null) _menu = GameObject.FindWithTag("PauseMenu");

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Title"))
        {
            _menu.SetActive(!_menu.activeSelf);
            Time.timeScale = _menu.activeSelf ? 0 : 1;
        }
            
    }

    private void OnQuitPressed(InputAction.CallbackContext context) {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Title"))
        {
            if (!_menu.activeSelf) _menu.SetActive(true);
            else SceneManager.LoadScene("Title");

            return;
        }
        
        QuitButton();
    }

    public void PlayButton() {
        SceneManager.LoadScene("Play");
    }

    public void QuitButton() {
        Application.Quit();
    }
}
