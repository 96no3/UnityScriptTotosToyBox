using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeyScript : MonoBehaviour
{
    public GameObject noteEffect;
    private float effectTime = 0;

    private AudioSource myAudio;

    public AudioClip se;            //プレイヤーが乗ったら鳴らす音
    [SerializeField] private float downSpeed = 2.0f;
    [SerializeField] private float upSpeed = 3.0f;

    private bool onPlayer = false;  //プレイヤーが乗っているか
    private float step;

    void Start()
    {
        effectTime = 0;
        myAudio = GetComponent<AudioSource>();
        noteEffect.SetActive(false);
    }

    void Update()
    {
        if (noteEffect.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 0.5f)
            {
                noteEffect.SetActive(false);
                effectTime = 0;
            }
        }

        if (onPlayer)
        {
            step = downSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -1.0f), step);
        }
        else
        {
            step = upSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            myAudio.PlayOneShot(se);
            noteEffect.SetActive(true);
            onPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            onPlayer = false;
        }
    }
}
