using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultNPC : MonoBehaviour
{
    public int id = 0;
    [HideInInspector] public bool isDead = true;

    private Animator anim;
    private AudioSource aud;

    [Header("SoundSE")]
    public AudioClip seHitEnemy;
    

    public void Start()
    {
        aud = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
    }

    public void SetIdle()
    {
        anim.SetTrigger("Idle");
    }

    public void SetDead()
    {
        aud.PlayOneShot(seHitEnemy);
        anim.SetTrigger("DeadTrigger");
    }
}
