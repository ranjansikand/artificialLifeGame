using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Image _bar;
    [SerializeField] float _maxLerpTime = 1f;

    float _maxHealth;
    float _currentHealth;
    float _previousPercentage;

    public void InitializeHealthbar(float health) {
        _bar.fillAmount = 1;
        _maxHealth = _currentHealth = health;
    }

    public void UpdateHealthbar(float health) {
        _bar.fillAmount = health/_maxHealth;
        _currentHealth = health;
    }

    IEnumerator AdjustBar(float newHealthPercentage) {
        float currentHealthPercentage = _previousPercentage > _currentHealth/_maxHealth ?
            _previousPercentage : _currentHealth/_maxHealth;
        float totalTime = _maxLerpTime * newHealthPercentage;
        float elapsedTime = 0;

        while (elapsedTime < totalTime) {
            _previousPercentage = _bar.fillAmount = 
                Mathf.Lerp(currentHealthPercentage, 
                newHealthPercentage, 
                elapsedTime/totalTime
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
