using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yrotation : MonoBehaviour
{
    public float rotSpeed = 10.0f;

    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
