using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _cooldownPanel;

    public Text Name { get { return _name; }}
    public Animator Animator { get { return _animator; }}  // Extend when selected



    public IEnumerator StartCooldown(float cooldownTime) {
        float elapsedTime = 0;
        _cooldownPanel.fillAmount = 1.0f;

        while (elapsedTime < cooldownTime) {
            _cooldownPanel.fillAmount = 1.0f - (elapsedTime/cooldownTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _cooldownPanel.fillAmount = 0f;
    }
}
