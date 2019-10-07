using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGimmic : MonoBehaviour
{
    [SerializeField] private float eventTime = 20.0f;
    [SerializeField] private Transform eventPos;
    public Transform eventCameraPos;
    private bool inEvent = false;
    
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
    
}
