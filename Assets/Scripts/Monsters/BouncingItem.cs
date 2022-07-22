using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingItem : MonoBehaviour
{
    [SerializeField] float rotationX = 35f;
    [SerializeField] float rotationY = 65f;
    [SerializeField] float rotationZ = 35f;


    void Update() {
        // Rotation Script
        transform.Rotate(
            Time.deltaTime * rotationX, 
            Time.deltaTime * rotationY, 
            Time.deltaTime * rotationZ, 
            Space.Self
        );
    }
}
