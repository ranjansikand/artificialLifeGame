using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] MonsterData _data;

    int _usesLeft = 4;

    public MonsterData Data { get { return _data; }}
    public int Uses {get { return _usesLeft; } set { _usesLeft = value; }}

    public void SetData(MonsterData newData, int uses = 4) {
        _data = newData;
        _usesLeft = uses;
    }

    public void Collected() {
        Invoke(nameof(DestroyThis), 0.05f);
    }

    private void DestroyThis() {
        // Play particle effect
        Destroy(gameObject);
    }
}
