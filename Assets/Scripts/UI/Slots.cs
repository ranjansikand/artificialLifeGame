using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public static Slots instance;

    [SerializeField] MonsterSlot[] _slots;

    MonsterSlot _selectedSlot;
    int _selectedHash;
    int _filledHash;

    void Awake() {
        instance = this;

        _selectedHash = Animator.StringToHash("Selected");
        _filledHash = Animator.StringToHash("Filled");

        for (int i = 0; i < _slots.Length; i++) {
            UpdateSlot(i);
        }
        SelectSlot(0);
    }

    public void UpdateSlot(int slot, MonsterData data = null) {
        if (data == null) {
            _slots[slot].Name.text = "";
            _slots[slot].Animator.SetBool(_filledHash, false);
        }
        else {
            _slots[slot].Animator.SetBool(_filledHash, true);
            _slots[slot].Name.text = data._monsterName;
        }
    }

    public void SelectSlot(int slot) {
        if (_selectedSlot != _slots[slot]) {
            // Switch this to animation
            _selectedSlot?.Animator.SetBool(_selectedHash, false);
            _selectedSlot = _slots[slot];
            _selectedSlot.Animator.SetBool(_selectedHash, true);
        }
    }

    public void CooldownSlot(int slot, float cooldownTime) {
        StartCoroutine(_slots[slot].StartCooldown(cooldownTime));
    }
}
