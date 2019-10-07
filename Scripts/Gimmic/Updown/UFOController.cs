using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    [SerializeField] private GameObject uiRide;
    private AudioSource aud;

    private enum UFOState
    {
        Stay,
        Move,
    }
    private UFOState state;

    private StageMgr stageMgr;

    [SerializeField] private GameObject movePos;
    [SerializeField] private Collider ufoStage;
    [SerializeField] private Transform[] trsPos;
    [SerializeField] private float sleepTime = 4.0f;
    [SerializeField] private float moveTime = 1.5f;      //1行程にかける時間

    private float diffTime = 0.0f;
    private int cnt = 1;
    private bool isReverse = false;     //true:帰り false:行き
    private Transform startPos;         //はじめの位置
    private Transform nextPos;          //次の移動位置

    private bool onPlayer = false;
    private bool isRide = false;
    private bool isStarted = false;
    private Rigidbody rbPlayer;
    private Collider colPlayer;
    private Player player;

    [Header("Input")]
    public string rideInput = "Player1_B";

    void Start()
    {
        //初期処理
        cnt = 1;
        diffTime = 0.0f;
        isReverse = false;
        onPlayer = false;
        isRide = false;
        isStarted = false;
        movePos.transform.parent = null;
        startPos = trsPos[0];
        nextPos = trsPos[1];
        state = UFOState.Stay;
        stageMgr = GameObject.FindWithTag("StageMgr").GetComponent<StageMgr>();
        aud = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!isStarted || stageMgr.gameState != StageMgr.GameState.Run)
        {
            if (aud.isPlaying)
            {
                aud.Stop();
            }            
            return;
        }

        if (onPlayer)
        {
            if (Input.GetButtonDown(rideInput) && !isRide)
            {   // gamepad Bまたはキーボードaを入力したら
                isRide = true;
                ChangePlayerMove();

                if (state != UFOState.Move)
                {
                    ChangeMoveState();
                }
            }
        }

        switch (state)
        {
            case UFOState.Stay:
                StayFixedUpdate();
                break;
            case UFOState.Move:
                MoveFixedUpdate();
                break;
            default:
                break;
        }
    }

    void StayFixedUpdate()
    {
        diffTime += Time.fixedDeltaTime;

        if (diffTime >= sleepTime)
        {
            ChangeMoveState();
        }
    }

    void MoveFixedUpdate()
    {
        diffTime += Time.fixedDeltaTime;

        if (isRide)
        {
            rbPlayer.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);            
        }

        if (diffTime > moveTime)
        {
            transform.position = nextPos.position;

            if (isReverse)
            {
                cnt--;

                if (cnt == -1)
                {
                    isReverse = false;                    
                    ChangeStayState();
                    if (onPlayer)
                    {
                        ChangePlayerStay();
                    }
                    cnt = 1;
                }
            }
            else
            {
                cnt++;

                if (cnt == trsPos.Length)
                {
                    isReverse = true;
                    ChangeStayState();
                    if (onPlayer)
                    {
                        ChangePlayerStay();
                    }
                    cnt--;
                }
            }

            startPos = nextPos;
            nextPos = trsPos[cnt];
            diffTime = 0.0f;
        }

        float rate = diffTime / moveTime;

        transform.position = Vector3.Lerp(startPos.position, nextPos.position, rate);
        transform.Rotate(0, 25.0f * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!player)
            {
                player = other.GetComponent<Player>();
            }

            if (!rbPlayer)
            {
                rbPlayer = other.GetComponent<Rigidbody>();
            }
            if (!colPlayer)
            {
                colPlayer = other.GetComponent<CapsuleCollider>();
            }
            if (!isStarted)
            {
                isStarted = true;
            }            
            onPlayer = true;                        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onPlayer = false;
        }
    }

    void ChangeStayState()
    {
        aud.Stop();
        diffTime = 0;
        state = UFOState.Stay;

        if (!uiRide.activeSelf)
        {
            uiRide.SetActive(true);
        }

        if (ufoStage.isTrigger)
        {
            ufoStage.isTrigger = false;
        }

        if (isRide)
        {
            isRide = false;
        }        
    }

    void ChangeMoveState()
    {
        aud.Play();
        diffTime = 0.0f;
        state = UFOState.Move;

        if (!ufoStage.isTrigger)
        {
            ufoStage.isTrigger = true;
        }

        if (uiRide.activeSelf)
        {
            uiRide.SetActive(false);
        }
    }

    // OnPlayer時の状態処理関数
    void ChangePlayerMove()
    {
        rbPlayer.velocity = Vector3.zero;

        if (!player.onUFO)
        {
            player.onUFO = true;
        }

        if (rbPlayer.useGravity)
        {
            rbPlayer.useGravity = false;
        }

        if (!colPlayer.isTrigger)
        {
            colPlayer.isTrigger = true;
        }
    }

    void ChangePlayerStay()
    {
        if (player.onUFO)
        {
            player.onUFO = false;
        }

        if (!rbPlayer.useGravity)
        {
            rbPlayer.useGravity = true;
        }

        if (colPlayer.isTrigger)
        {
            colPlayer.isTrigger = false;
        }        
    }
}
