using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private Slider _volumeControl;
    [SerializeField] private Image _buttonImage;
    private static float _masterVolume;

    public static float MasterVolume { get { return _masterVolume; }}

    [SerializeField] Sprite _sound, _muted;
    bool _mute = false;

    AudioSource _musicSource;


    private void Awake() {
        _volumeControl = GetComponentInChildren<Slider>();
        _masterVolume = PlayerPrefs.GetFloat("Vol", 0.5f);
        _musicSource = Camera.main.gameObject.GetComponent<AudioSource>();
    }

    private void Start() {
        _volumeControl.value = _masterVolume;
    }

    public void UpdateVolume() {
        _masterVolume = _volumeControl.value;
        PlayerPrefs.SetFloat("Vol", _masterVolume);
        _musicSource.volume = _masterVolume;

        UpdateSprite();
    }

    public void ToggleAudio() {
        if (_mute) {
            UpdateVolume();
            _mute = false;
        }
        else {
            _mute = true;
            _masterVolume = 0;
            _musicSource.volume = 0;
            UpdateSprite();
        }
    }

    private void UpdateSprite() {
        if (_masterVolume == 0) _buttonImage.sprite = _muted;
        else _buttonImage.sprite = _sound;
    }


}
