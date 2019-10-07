using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlopeMiniCar : MonoBehaviour
{
    private AudioSource aud;

    public Transform endPos;

    private Vector3 startPos;
    private NavMeshAgent agent;

    private bool isRespawn = false;
    private float time = 0;

    public enum State
    {
        None,
        Move
    }
    [HideInInspector] public State state;

    [Header("parameter")]
    [SerializeField] private float respawnTime = 1.0f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float acceleration = 1.0f;

    void Start()
    {
        isRespawn = false;
        time = 0;
        aud = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        agent.autoBraking = false;
        endPos.transform.parent = null;
        state = State.Move;
    }

    void Update()
    {
        switch (state)
        {
            case State.None:
                UpdateNone();
                break;
            case State.Move:
                UpdateMove();
                break;
            default:
                break;
        }
    }

    void UpdateNone()
    {
        agent.speed = 0;
        agent.acceleration = 0;
        agent.velocity = Vector3.zero;
        aud.Stop();
    }

    void UpdateMove()
    {
        if (isRespawn)
        {
            time += Time.deltaTime;
        }

        if (time > respawnTime)
        {
            isRespawn = false;
            transform.position = startPos;
            time = 0;
            agent.speed = speed;
            agent.acceleration = acceleration;
            aud.Play();
        }

        agent.SetDestination(endPos.position);

        if (agent.remainingDistance < 0.1f)
        {
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            isRespawn = true;
            if (aud.isPlaying)
            {
                aud.Stop();
            }
        }
    }

    public void Restart()
    {        
        aud.Play();
        agent.speed = speed;
        agent.acceleration = acceleration;
        state = State.Move;
    }

    public void StopState()
    {
        aud.Stop();
        transform.position = startPos;
        isRespawn = false;
        time = 0;
    }
}
