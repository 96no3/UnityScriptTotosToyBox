using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookControll : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Stopper")
        {
            rb.isKinematic = true;
            col.isTrigger = false;
        }
    }

}
