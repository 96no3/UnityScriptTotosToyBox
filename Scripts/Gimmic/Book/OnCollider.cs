using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollider : MonoBehaviour
{
    [SerializeField] private Collider col;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            col.enabled = true;
        }
    }
}
