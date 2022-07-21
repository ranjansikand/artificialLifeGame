using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCard : MonoBehaviour
{
    [Tooltip("Filled bars in Monster UI")]
    [SerializeField] Image _health, _damage, _sightRange, _buildTime;


    static float maxHealth = 12, maxdamage = 10, maxSight = 10, maxBuildTime = 8;

    MonsterData _data;
    bool _showCard = false;

    void Awake() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false); 
        }
    }

    public void SaveData(MonsterData data) {
        _showCard = data != null;
        if (_data != data) _data = data;

        Invoke(nameof(UpdateCard), 0.2f);
    }

    public void DeselectCard() {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }

    public void UpdateCard() {
        if (_showCard == false ||_data == null) {
            foreach(Transform child in transform) {
                child.gameObject.SetActive(false);
            }
        }
        else {
            foreach(Transform child in transform) {
                child.gameObject.SetActive(true);
            }
            _health.fillAmount = _data._health/maxHealth;
            _damage.fillAmount = _data._damage/maxdamage;
            _sightRange.fillAmount = _data._sightRange/maxSight;
            _buildTime.fillAmount = _data._spawnTime/maxBuildTime;
        }
    }
}
