using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    public void Damage(float damage) {

    }

    public bool Alive() {
        return true;
    }
}
