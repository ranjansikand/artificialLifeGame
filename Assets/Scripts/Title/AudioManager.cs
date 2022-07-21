using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private Slider _volumeControl;
    private static float _masterVolume;

    public static float MasterVolume { get { return _masterVolume; }}


    private void Awake() {
        DontDestroyOnLoad(this.gameObject);

        _masterVolume = PlayerPrefs.GetFloat("Vol", 0.5f);
    }
    
    private void OnLevelWasLoaded (int level) {
        _volumeControl = GameObject.FindWithTag("Volume").GetComponent<Slider>();
        _volumeControl.value = _masterVolume;
    }

    public void UpdateVolume() {
        _masterVolume = _volumeControl.value;
        PlayerPrefs.SetFloat("Vol", _masterVolume);
    }
}
