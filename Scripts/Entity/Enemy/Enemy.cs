using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        None,
        Stay,
        Move,
        Attack,
        Event
    }
    [HideInInspector] public EnemyState enemyState;

    //手のコライダー
    public Collider handCollider;

    [HideInInspector] public GameObject targetNPC;
    private NavMeshAgent agent;
    private Animator anim;
    private AudioSource aud;

    public GameObject pointObject;
    public Transform[] points;
    private int destPoint = 0;

    [Header("se")]
    public AudioClip seAttack;
    public AudioClip seIdle;

    [Header("parameter")]
    public float speed = 0.5f;
    public float acceleration = 1.0f;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public bool isIdle = false;
    [HideInInspector] public bool isMove = false;

    private float idleAnimTime = 4.21f;
    private float attackAnimTime = 2.29f;
    private float time = 0;

    void Start()
    {
        isAttack = false;
        isIdle = false;
        isMove = false;        
        time = 0;
        destPoint = 0;
        pointObject.transform.parent = null;
        aud = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        handCollider.enabled = false;
        agent.autoBraking = false;
        anim.SetTrigger("MoveTrigger");
        enemyState = EnemyState.Move;
    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.None:
                UpdateNone();
                break;
            case EnemyState.Stay:
                UpdateStay();
                break;
            case EnemyState.Move:
                UpdateMove();
                break;
            case EnemyState.Attack:
                UpdateAttack();
                break;
            case EnemyState.Event:
                UpdateEvent();
                break;
            default:
                enemyState = EnemyState.None;
                break;
        }
    }

    void UpdateNone()
    {
        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
        isAttack = false;
        isIdle = false;
        isMove = true;
    }

    void UpdateStay()
    {
        time += Time.deltaTime;
        if (isIdle)
        {
            PlaySeIdle();
            anim.SetTrigger("IdleTrigger");
            isIdle = false;
        }
        if(time > idleAnimTime)
        {
            isMove = true;
            enemyState = EnemyState.Move;
            time = 0;
        }
        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
    }

    void UpdateMove()
    {
        if (isMove)
        {
            anim.SetTrigger("MoveTrigger");
            isMove = false;
        }
        agent.speed = speed;
        agent.acceleration = acceleration;
        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択します
        if (!agent.pathPending && agent.remainingDistance < 0.05f)
        {
            GotoNextPoint();
        }            
    }

    void UpdateAttack()
    {
        time += Time.deltaTime;
        if (isAttack)
        {
            isAttack = false;
            anim.SetTrigger("AttackTrigger");
        }
        if (time > attackAnimTime)
        {
            isIdle = true;
            enemyState = EnemyState.Stay;
            time = 0;
        }

        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
    }

    void UpdateEvent()
    {
        agent.speed = speed * 2.0f;
        agent.SetDestination(targetNPC.transform.position);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (enemyState == EnemyState.None) return;

        if (other.tag == "NPC" || other.tag == "Player")
        {
            if (enemyState != EnemyState.Attack && !anim.IsInTransition(0))
            {
                time = 0;
                isAttack = true;
                enemyState = EnemyState.Attack;
                aud.PlayOneShot(seAttack);
            }            
        }
    }

    public void SetCollider(bool flag)
    {
        handCollider.enabled = flag;
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        agent.destination = points[destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % points.Length;
    }

    public void PlaySeIdle()
    {
        aud.PlayOneShot(seIdle);
    }

    public void SetMoveAnim()
    {
        anim.SetTrigger("MoveTrigger");
    }
}
