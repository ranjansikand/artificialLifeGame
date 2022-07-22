using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject _controlsPanel;

    [SerializeField] AudioClip _clickSound;

    AudioSource _audioSource;
    WaitForSeconds _delay = new WaitForSeconds(0.25f);

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayClicked() {
        SceneManager.LoadScene("Play");
    }

    public void OnControlsMenu() {
        PlaySound();
        _controlsPanel.SetActive(true);
    }
    
    public void OnBackButton() {
        PlaySound();
        _controlsPanel.SetActive(false);
    }

    public void OnQuitButton() {
        Application.Quit();
    }

    private void PlaySound() {
        _audioSource.PlayOneShot(_clickSound, AudioManager.MasterVolume);
    }
}
