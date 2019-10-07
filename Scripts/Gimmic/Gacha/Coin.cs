using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Coin : MonoBehaviour
{
    public enum CoinState
    {
        Stay,
        Follow,
    }
    [HideInInspector] public CoinState coinState;

    [HideInInspector] public Vector3 targetPos;

    [Header("Effect")]
    public GameObject hitEffectPrefab;

    [Header("parameter")]
    public float speed = 1.0f;
    public float acceleration = 1.5f;

    private float effectTime = 0;
    private const float rotateSpeed = 100;

    private NavMeshAgent agent;
    private AudioSource aud;

    [Header("SoundSE")]
    public AudioClip seHitPlayer;

    private Vector3 spawnPos;

    void Start()
    {
        transform.parent = null;
        coinState = CoinState.Stay;
        aud = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        hitEffectPrefab.SetActive(false);
        targetPos = GameObject.FindWithTag("CoinTarget").transform.position;
        spawnPos = transform.position;
        GameInstance.Instance.spawnPosList.Add(spawnPos);
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

        switch (coinState)
        {
            case CoinState.Stay:
                UpdateStay();
                break;
            case CoinState.Follow:
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
        if(other.tag == "Player" && coinState == CoinState.Stay)
        {
            GameInstance.Instance.coinList.Add(this);
            aud.PlayOneShot(seHitPlayer);
            coinState = CoinState.Follow;
            hitEffectPrefab.SetActive(true);
        }
    }
}
