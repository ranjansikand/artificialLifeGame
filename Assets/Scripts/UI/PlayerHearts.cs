using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHearts : MonoBehaviour
{
    public static PlayerHearts instance;

    [SerializeField] Animator[] _hearts;

    int _loseHash;
    int _currentHealth = 4;

    void Awake() {
        instance = this;

        _loseHash = Animator.StringToHash("Lose");
    }

    public void LoseHealth() {
        _currentHealth -= 1;
        _hearts[_currentHealth].SetBool(_loseHash, true);
    }
}
