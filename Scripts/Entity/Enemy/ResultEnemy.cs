using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResultEnemy : MonoBehaviour
{    
    private Animator anim;
    private AudioSource aud;
    
    [Header("se")]
    public AudioClip seAttack;
    public AudioClip seIdle;

    void Start()
    {        
        aud = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();        
    }

    public void SetIdleAnim()
    {
        anim.SetTrigger("IdleTrigger");
    }

    public void SetMoveAnim()
    {
        anim.SetTrigger("MoveTrigger");
    }

    public void SetAttackAnim()
    {
        anim.SetTrigger("AttackTrigger");
    }
}
