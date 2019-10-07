using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTriggerScript : MonoBehaviour
{
    public PulleyScript pulley;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            pulley.SetValue(true);
        }
    }
}