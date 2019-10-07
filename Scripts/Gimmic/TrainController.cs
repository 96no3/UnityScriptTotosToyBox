using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TrainController : MonoBehaviour
{
    private AudioSource aud;
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Vector3 startPos;

    public enum State
    {
        None,
        Move
    }
    [HideInInspector] public State state;

    [Header("parameter")]
    public float speed = 1.0f;
    public float acceleration = 2.0f;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;
        aud.Play();
        GotoNextPoint();
        startPos = transform.position;
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
        agent.speed = speed;
        agent.acceleration = acceleration;

        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択します
        if (!agent.pathPending && agent.remainingDistance < 0.05f)
            GotoNextPoint();
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
    }
}
