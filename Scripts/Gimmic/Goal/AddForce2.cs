using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce2 : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Vector3 forceDir = Vector3.zero;
    public bool isActive = false;
    [SerializeField] private float force = 0;
    private float time = 0;

    void Start()
    {
        time = 0;
        rb = GetComponent<Rigidbody>();
        forceDir = forceDir.normalized;
    }

    void Update()
    {
        time += Time.deltaTime;

        if(isActive)
        {
            rb.AddForce(forceDir * force, ForceMode.Impulse);
        }
    }
}
