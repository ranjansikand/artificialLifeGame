using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
    public static Slots instance;

    [SerializeField] Text[] _name;

    void Awake() {
        instance = this;
    }

    public void UpdateSlot(int slot, MonsterData data) {
        if (data == null) _name[slot].text = "Empty";
        else _name[slot].text = data._monsterName;
    }
}
