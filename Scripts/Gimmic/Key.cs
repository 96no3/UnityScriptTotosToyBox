using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Key : MonoBehaviour
{
    private int id = 900;
    private StageMgr stageMgr;

    public enum KeyState
    {
        Stay,
        Follow,
    }
    [HideInInspector] public KeyState keyState;

    [HideInInspector] public Vector3 targetPos;

    [Header("Effect")]
    public GameObject hitEffectPrefab;
    public GameObject idleFXPrefab;

    [Header("parameter")]
    public float speed = 1.0f;
    public float acceleration = 1.5f;

    private float effectTime = 0;
    private const float rotateSpeed = 100;

    private NavMeshAgent agent;
    private AudioSource aud;

    [Header("SoundSE")]
    public AudioClip seHitPlayer;

    void Start()
    {
        effectTime = 0;
        keyState = KeyState.Stay;
        aud = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        hitEffectPrefab.SetActive(false);
        targetPos = GameObject.FindWithTag("CoinTarget").transform.position;
        stageMgr = GameObject.FindGameObjectWithTag("StageMgr").GetComponent<StageMgr>();
    }
    

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        if (hitEffectPrefab.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 0.5f)
            {
                hitEffectPrefab.SetActive(false);
                effectTime = 0;
            }
        }

        switch (keyState)
        {
            case KeyState.Stay:
                UpdateStay();
                break;
            case KeyState.Follow:
                UpdateFollow();
                break;
            default:
                break;
        }
    }

    void UpdateStay()
    {
        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
    }

    void UpdateFollow()
    {
        targetPos = GameObject.FindWithTag("CoinTarget").transform.position;
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.SetDestination(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && keyState == KeyState.Stay)
        {
            idleFXPrefab.SetActive(false);
            if (stageMgr.missionMgr.CheckID(stageMgr.ui.currentMissionNum, id))
            {
                GameInstance.Instance.hasKey = true;
                stageMgr.missionMgr.mission[stageMgr.ui.currentMissionNum].state = MissionMgr.MissionState.CLEAR;                
            }
            //other.GetComponent<Player>().ui.CheckKey();            
            aud.PlayOneShot(seHitPlayer);
            keyState = KeyState.Follow;
            hitEffectPrefab.SetActive(true);
        }
    }
}
