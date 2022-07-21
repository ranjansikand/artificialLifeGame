using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCounter : MonoBehaviour
{
    [SerializeField] Text counter;
    int number = 0;
    const int maxNumber = 4;

    void Start()
    {
        PlayerController.monsterCreated += Add;
        MonsterController.monsterDied += Subtract;

        counter.text = number.ToString() + " / " + maxNumber;
    }

    void Add() {
        number += 1;
        counter.text = number.ToString() + " / " + maxNumber;
    }

    void Subtract() {
        number -= 1;
        counter.text = number.ToString() + " / " + maxNumber;
    }
}
