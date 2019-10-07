using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private float timer = 1;
    private float resetTime = 0;
    private bool isUp = false;
    private Rigidbody rb;

    public float upForce = 4.0f;
    private BubbleSpawner spawn;

    void Start()
    {
        isUp = false;
        timer = 1;
        resetTime = 0;
        rb = GetComponent<Rigidbody>();
        float scale = Random.Range(0.05f, 0.15f);
        transform.localScale = new Vector3(scale, scale, scale);
        spawn = GetComponentInParent<BubbleSpawner>();
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (isUp)
        {
            resetTime += Time.deltaTime;
            if (resetTime >= 15.0f)
            {
                rb.velocity = Vector3.zero;
                transform.position = spawn.vec;
                resetTime = 0;
                isUp = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Wall")
        {
            rb.velocity = Vector3.zero;
            transform.position = spawn.vec;
            resetTime = 0;
            isUp = false;
        }        
    }

    public void UpForce()
    {
        rb.AddForce(Vector3.up * upForce);
        isUp = true;
    }

    public void Fukitobasi(Vector3 vec)
    {
        if(timer <= 0) {
            rb.AddForce(vec);
            timer = 1;
        }
    }
}
