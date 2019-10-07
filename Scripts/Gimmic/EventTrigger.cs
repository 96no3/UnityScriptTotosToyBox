using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{    
    [SerializeField] private float interval = 1.0f;
    [SerializeField] private float eventTime = 7.0f;
    [SerializeField] private NPC eventNPC;
    [SerializeField] private Enemy eventEnemy;
    [SerializeField] private Transform eventPos;
    [SerializeField] private Transform eventCameraPos;

    private float time = 0;
    private bool inEvent = false;

    private void Start()
    {
        time = 0;
        inEvent = false;
    }

    void Update()
    {
        if (inEvent)
        {
            if (time == 0)
            {
                SpawnNPC();
            }
            if (time >= 0)
            {
                time += Time.deltaTime;
            }
            if (time >= interval * 0.1f)
            {
                eventNPC.eventPos = eventPos.position;
                eventNPC.npcState = NPC.NPCState.Event;
            }

            if (time >= interval)
            {
                SpawnEnemy();
                time = -1;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!inEvent)
            {
                inEvent = true;
                //メインカメラを取得する
                ThirdPersonCamera cameraObject = GameObject.FindWithTag("MainCamera").GetComponent<ThirdPersonCamera>();
                cameraObject.eventPos = eventCameraPos;
                cameraObject.eventLookPos = eventPos;
                cameraObject.isEvent = true;
                StageMgr stageMgr = GameObject.FindWithTag("StageMgr").GetComponent<StageMgr>();
                stageMgr.gameState = StageMgr.GameState.Event;
                stageMgr.eventTime = eventTime;
            }
        }
    }

    void SpawnNPC()
    {
        eventNPC.gameObject.SetActive(true);
        eventPos.transform.parent = null;
        eventNPC.transform.parent = null;
    }

    void SpawnEnemy()
    {        
        eventEnemy.transform.parent = null;
        eventEnemy.gameObject.SetActive(true);
        eventEnemy.targetNPC = eventNPC.gameObject;
        eventEnemy.enemyState = Enemy.EnemyState.Event;
    }
}
