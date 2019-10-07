using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falldown : MonoBehaviour
{
    private SoundManager sound;

    public float fallTime = 0.5f;
    private Rigidbody rb;
    private float time = 0;
    private bool onPlayer = false;

    private void Start()
    {
        time = 0;
        onPlayer = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    void Update()
    {
        if (onPlayer)
        {
            time += Time.deltaTime;
            if(time >= fallTime)
            {
                rb.isKinematic = false;
                time = 0;
                sound.PlaySE(SoundManager.Sound.FallBook);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!onPlayer)
            {
                onPlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (onPlayer)
            {
                onPlayer = false;
                time = 0;
            }
        }
    }
}
