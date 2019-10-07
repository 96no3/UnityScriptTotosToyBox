using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public int id = 0;
    public void SetId(int num) { id = num; }
    public int GetId() { return id; }
    private bool isMissionChecked = false;

    public Transform[] points;
    private int destPoint = 0;

    private StageMgr stageMgr;
    private SoundManager sound;

    public enum NPCState
    {
        None,
        Help,
        Follow,
        Rescued,
        Escape,
        Dead,
        Event,
    }
    [HideInInspector] public NPCState npcState;

    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 eventPos = Vector3.zero;
    private Vector3 startPos = Vector3.zero;   //はじめの位置
    private Vector3 nextPos = Vector3.zero;    //次の移動位置

    [Header("Effect")]
    public GameObject hitEffectPrefab;
    public GameObject hitGimmmick;
    public GameObject idleEffect;

    [Header("parameter")]
    public float speed = 1.0f;
    public float acceleration = 1.5f;
    public float upGoalForce = 1.0f;
    public float knockbackForce = 0.1f;

    private float effectTime = 0;
    private float time = 0;
    private float resetTime = 0;
    private float goalTime = 0;
    [HideInInspector] public bool isActive = false;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public AudioSource aud;
    [HideInInspector] public Rigidbody rb;

    [Header("SoundSE")]
    public AudioClip seHitPlayer;
    public AudioClip seHitGimmick;
    public AudioClip seHitEnemy;
    

    public void Start()
    {
        isMissionChecked = false;
        destPoint = 0;
        effectTime = 0;
        time = 0;
        resetTime = 0;
        goalTime = 0;
        stageMgr = GameObject.FindGameObjectWithTag("StageMgr").GetComponent<StageMgr>();
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        npcState = NPCState.Help;
        aud = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        hitEffectPrefab.SetActive(false);
        idleEffect.SetActive(false);
        hitGimmmick.SetActive(false);
        NPCInit();
    }

    public virtual void NPCInit()
    {        
    }

    void Update()
    {
        if (time <= 1.51f)
        {
            time += Time.deltaTime;
        }

        if (hitEffectPrefab.activeInHierarchy || hitGimmmick.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 0.5f)
            {
                hitEffectPrefab.SetActive(false);
                hitGimmmick.SetActive(false);
                effectTime = 0;
            }
        }

        switch (npcState)
        {
            case NPCState.None:
                UpdateNone();
                break;
            case NPCState.Help:
                UpdateHelp();
                break;
            case NPCState.Follow:
                UpdateFollow();
                break;
            case NPCState.Escape:
                UpdateEscape();
                break;
            case NPCState.Dead:
                UpdateDead();
                break;
            case NPCState.Event:
                UpdateEvent();
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (npcState)
        {            
            case NPCState.Rescued:
                FixedUpdateRescued();
                break;
            default:
                break;
        }
    }

    private void UpdateNone()
    {
        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
    }

    private void UpdateHelp()
    {
        if (!isActive)
        {
            resetTime += Time.deltaTime;
            idleEffect.SetActive(true);
            agent.speed = 0;
            agent.acceleration = 0;
            agent.velocity = Vector3.zero;            
            anim.SetBool("IsJump", false);
            anim.SetBool("IsMove", false);
            if (resetTime > 1.0f)
            {
                rb.velocity = Vector3.zero;
                resetTime = 0;
            }            
        }
        else
        {
            idleEffect.SetActive(true);
            NPCUpdateHelp();
        }
    }

    public virtual void NPCUpdateHelp()
    {
    }

    private void UpdateFollow()
    {
        idleEffect.SetActive(false);
        anim.SetBool("IsMove", true);
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.SetDestination(targetPos);
        NPCUpdateFollow();
    }

    public virtual void NPCUpdateFollow()
    {
    }

    private void FixedUpdateRescued()
    {
        goalTime += Time.fixedDeltaTime;
        agent.enabled = false;
        rb.velocity = Vector3.zero;
        Vector3 goalPos = GameObject.FindWithTag("GoalPos").transform.position;
        Vector3 vec = new Vector3(goalPos.x - transform.position.x, goalPos.y - transform.position.y, goalPos.z - transform.position.z);

        rb.AddForce(vec.normalized * upGoalForce, ForceMode.Impulse);

        if (goalTime > 4.0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateEscape()
    {
        idleEffect.SetActive(false);
        anim.SetBool("IsJump", true);
        NPCUpdateEscape();
    }

    public virtual void NPCUpdateEscape()
    {
    }    

    private void UpdateDead()
    {
        idleEffect.SetActive(false);
        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
        anim.SetTrigger("DeadTrigger");
    }

    void UpdateEvent()
    {
        idleEffect.SetActive(false);
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.SetDestination(eventPos);
        anim.SetBool("IsMove", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(time >= 1.5f)
        {
            if (npcState == NPCState.Help && other.tag == "Player")
            {
                StartCoroutine("HitPlayer");
            }

            if (npcState != NPCState.Dead && other.tag == "Enemy")
            {
                rb.AddForce(other.transform.forward * knockbackForce, ForceMode.Impulse);
                StartCoroutine("HitEnemy");
            }

            if (npcState == NPCState.Follow && other.tag == "Mover")
            {
                StartCoroutine("HitMover");
            }            
        }

        if (npcState != NPCState.Dead && other.tag == "EnemyHand")
        {
            StartCoroutine("HitEnemyHand");
        }
    }

    public void ChangeStateHelp()
    {
        npcState = NPCState.Help;
    }

    IEnumerator HitPlayer()
    {
        if (isActive)
        {
            isActive = false;
        }
        time = 0;
        aud.PlayOneShot(seHitPlayer);
        hitEffectPrefab.SetActive(true);
        yield return null;
        GameInstance.Instance.helpedNpcList.Add(this);
        stageMgr.ui.HelpedNumAddNpc();
        yield return null;        

        if (!isMissionChecked)
        {
            if (stageMgr.missionMgr.CheckID(stageMgr.ui.currentMissionNum, id))
            {
                isMissionChecked = true;

                if (stageMgr.missionMgr.CheckNpc(stageMgr.ui.currentMissionNum))
                {
                    stageMgr.missionMgr.mission[stageMgr.ui.currentMissionNum].state = MissionMgr.MissionState.CLEAR;
                    stageMgr.isMissionClear = true;
                    sound.PlaySE(SoundManager.Sound.MissionClear);

                    if (stageMgr.ui.currentMissionNum == 2 || stageMgr.ui.currentMissionNum == 5)
                    {
                        stageMgr.ui.MissionClearAnim(0);
                    }
                    else if (stageMgr.ui.currentMissionNum == 7)
                    {
                        stageMgr.ui.MissionClearAnim(3);
                    }
                    else if (stageMgr.ui.currentMissionNum == 3)
                    {
                        stageMgr.ui.MissionClearAnim(2);
                    }
                    else
                    {
                        stageMgr.ui.MissionClearAnim(1);
                    }
                    yield return null;
                }
            }
        }

        npcState = NPCState.Follow;
        yield return null;
    }

    IEnumerator HitMover()
    {
        time = 0;
        aud.PlayOneShot(seHitGimmick);
        hitGimmmick.SetActive(true);
        yield return null;
        
        GameInstance.Instance.helpedNpcList.Remove(this);
        stageMgr.ui.HelpedNumCarHit();        
        yield return null;

        npcState = NPCState.Escape;
        yield return null;
    }

    IEnumerator HitEnemy()
    {
        time = 0;
        aud.PlayOneShot(seHitGimmick);
        hitGimmmick.SetActive(true);
        yield return null;

        if (npcState == NPCState.Follow)
        {
            GameInstance.Instance.helpedNpcList.Remove(this);
            stageMgr.ui.HelpedNumEnemyHit();
            yield return null;
        }
        
        npcState = NPCState.Help;        
        yield return null;
    }

    IEnumerator HitEnemyHand()
    {
        aud.PlayOneShot(seHitEnemy);
        yield return null;

        if (npcState == NPCState.Follow)
        {
            GameInstance.Instance.helpedNpcList.Remove(this);
            stageMgr.ui.HelpedNumEnemyHit();
            yield return null;
        }

        npcState = NPCState.Dead;
        yield return null;
    }

    public void GotoNextPoint()
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
}
