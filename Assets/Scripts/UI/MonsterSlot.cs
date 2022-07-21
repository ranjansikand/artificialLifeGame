using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlot : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _cooldownPanel;
    [SerializeField] private MonsterCard _card;

    public Text Name { get { return _name; }}
    public Animator Animator { get { return _animator; }}  // Extend when selected
    public MonsterCard Card { get { return _card; }}



    private void Awake() {
        if (_card == null) _card = GetComponentInChildren<MonsterCard>();
    }

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
