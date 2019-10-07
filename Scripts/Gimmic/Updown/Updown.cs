using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updown : MonoBehaviour
{
    [SerializeField] private GameObject uiRide;
    [SerializeField] private Collider stage;

    [HideInInspector] public enum STATE
    {
        STAY,
        MOVE,
    }
    [HideInInspector] public STATE state = STATE.STAY;

    private StageMgr stageMgr;

    private Rigidbody rbPlayer;
    private Player player;
    private bool isReverse = false;
    [HideInInspector] public bool onPlayer = false;
    private bool isRide = false;
    private Vector3 startPos;
    [Header("Parameter")]
    [SerializeField] private float maxHeight = 2.0f;
    [SerializeField] private float speed = 0.6f;

    [Header("Input")]
    public string rideInput = "Player1_B";

    void Start()
    {
        startPos = transform.position;
        stageMgr = GameObject.FindWithTag("StageMgr").GetComponent<StageMgr>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        onPlayer = false;
        isRide = false;
        isReverse = false;
        state = STATE.STAY;
    }


    private void Update()
    {
        if (stageMgr.gameState != StageMgr.GameState.Run) return;

        if (transform.position.y > maxHeight && state == STATE.MOVE)
        {
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);

            if (!isReverse)
            {
                isReverse = true;
                ChangeStayState();
                ChangePlayerStay();
            }
        }

        if(transform.position.y <= startPos.y && state == STATE.MOVE)
        {
            transform.position = startPos;

            if (isReverse)
            {
                isReverse = false;
                ChangeStayState();
                ChangePlayerStay();
            }
        }

        if (onPlayer)
        {
            if (Input.GetButtonDown(rideInput) && !isRide)
            {   // gamepad Bまたはキーボードaを入力したら
                isRide = true;                

                if (state != STATE.MOVE)
                {
                    ChangeMoveState();
                    ChangePlayerMove();
                }
            }
        }
    }


    void FixedUpdate()
    {
        if (stageMgr.gameState != StageMgr.GameState.Run) return;

        switch (state)
        {
            case STATE.MOVE:
                MoveFixedUpdate();
                break;
            case STATE.STAY:
                StayFixedUpdate();
                break;
            default:
                break;
        }
    }

    private void MoveFixedUpdate()
    {
        if (isRide)
        {
            rbPlayer.position = new Vector3(transform.position.x, transform.position.y + 0.025f, transform.position.z);            
        }

        if (!isReverse)
        {
            transform.Translate(0, speed * Time.fixedDeltaTime, 0);
        }
        else
        {
            transform.Translate(0, -speed * 4.0f * Time.fixedDeltaTime, 0);
        }
    }

    private void StayFixedUpdate()
    {        
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {     
            if (!onPlayer)
            {
                onPlayer = true;
            }            
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
        if (!uiRide.activeSelf)
        {
            uiRide.SetActive(true);
        }
        if (stage.isTrigger)
        {
            stage.isTrigger = false;
        }
        if (isRide)
        {
            isRide = false;
        }
        
        state = STATE.STAY;
    }

    public void ChangeMoveState()
    {
        state = STATE.MOVE;

        if (uiRide.activeSelf)
        {
            uiRide.SetActive(false);
        }
        if (!stage.isTrigger)
        {
            stage.isTrigger = true;
        }
    }

    // OnPlayer時の状態処理関数
    void ChangePlayerMove()
    {
        rbPlayer.velocity = Vector3.zero;

        if (!player.onUp)
        {
            player.onUp = true;
        }

        if (rbPlayer.useGravity)
        {
            rbPlayer.useGravity = false;
        }
    }

    void ChangePlayerStay()
    {
        if (player.onUp)
        {
            player.onUp = false;
        }

        if (!rbPlayer.useGravity)
        {
            rbPlayer.useGravity = true;
        }
    }
}
