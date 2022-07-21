using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    PlayerInput _playerInput;

    void Awake()
    {
        _playerInput = new PlayerInput();

        DontDestroyOnLoad(this.gameObject);

        _playerInput.Menu.ToggleMenu.performed += OnToggleMenu;
    }

    private void OnEnable() { _playerInput.Enable(); }
    private void OnDisable() { _playerInput.Disable(); }

    private void OnToggleMenu(InputAction.CallbackContext context) {
        // if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Title))
        // _menu.SetActive(!_menu.activeSelf);
    }

    private void OnQuitPressed(InputAction.CallbackContext context) {
        // if (!_menu.activeSelf) _menu.SetActive(true);
        // else SceneManager.LoadScene("Title");
    }

    public void PlayButton() {
        SceneManager.LoadScene("Play");
    }
}
