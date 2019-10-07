using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [Header("Effect")]
    public GameObject hitGimmmick;
        
    [Header("parameter")]
    private float effectTime = 0;
    private float audioTime = 0;

    private AudioSource aud;
    [Header("SoundSE")]
    public AudioClip seHit;
    public AudioClip seMove;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        hitGimmmick.SetActive(false);
    }

    private void Update()
    {
        audioTime += Time.deltaTime;
        if(audioTime > 1.0f)
        {
            aud.PlayOneShot(seMove);
            audioTime = 0;
        }

        if (hitGimmmick.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 0.5f)
            {
                hitGimmmick.SetActive(false);
                effectTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC" || other.tag == "Player")
        {
            aud.PlayOneShot(seHit);
            hitGimmmick.SetActive(true);
        }
    }
}
