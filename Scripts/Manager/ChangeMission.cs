using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMission : MonoBehaviour
{
    private SoundManager sound;
    private StageMgr stageMgr;
    private MissionMgr missionMgr;
    private BGMMgr bgm;
    [SerializeField] private bool isTimeline = false;
    [SerializeField] private bool isLast = false;

    [SerializeField] private GameObject timeline;
    [SerializeField] private GameObject[] spawnNPC;    
    [SerializeField] private int missionNum = 0;
    private bool isFirst = true;
    private bool isMission = false;
    private float missionAnimTime = 2.30f;
    private float time = 0;
    [SerializeField] private float lastAnimTime = 2.30f;

    void Start()
    {
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        stageMgr = GameObject.FindGameObjectWithTag("StageMgr").GetComponent<StageMgr>();
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        missionMgr = GameObject.FindGameObjectWithTag("MissionMgr").GetComponent<MissionMgr>();
        isFirst = true;
        isMission = false;
        time = 0;
        for (int i = 0; i < spawnNPC.Length; ++i)
        {
            spawnNPC[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!isLast && !isTimeline && isMission)
        {
            time += Time.deltaTime;
            if (time > missionAnimTime)
            {               
                time = 0;
                isMission = false;
                missionMgr.ui.MissionBarImage(missionNum);
                stageMgr.isMissionCenter = false;
            }
        }

        if (isLast && isMission)
        {
            time += Time.deltaTime;
            if (time > lastAnimTime + 1.0f)
            {
                time = 0;
                isMission = false;
                missionMgr.mission[missionNum].state = MissionMgr.MissionState.PLAY;
                for (int i = 0; i < spawnNPC.Length; ++i)
                {
                    spawnNPC[i].SetActive(true);
                }
                stageMgr.isTimelineEnd = true;
                gameObject.SetActive(false);
            }
        }

        if (isTimeline && isMission && !isLast)
        {
            time += Time.deltaTime;
            if (time > (lastAnimTime + 0.1f))
            {
                time = 0;
                isMission = false;
                for (int i = 0; i < spawnNPC.Length; ++i)
                {
                    spawnNPC[i].SetActive(true);
                }
                missionMgr.ui.MissionBarImage(missionNum);
                stageMgr.isTimelineEnd = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            missionMgr.ui.currentMissionNum = missionNum;

            if (isLast && isFirst)
            {
                bgm.ChangeBGM(5);
                sound.PlaySE(SoundManager.Sound.MissionStart);
                other.GetComponent<TotoFace>().ChangeFace(2);
                isFirst = false;
                isMission = true;
                timeline.SetActive(true);
                stageMgr.gameState = StageMgr.GameState.Tips;
                return;
            }

            if (isTimeline && isFirst && !isLast)
            {
                if (missionNum == 4)
                {
                    other.GetComponent<TotoFace>().ChangeFace(3);
                    bgm.ChangeBGM(4);
                }
                else
                {
                    other.GetComponent<TotoFace>().ChangeFace(0);
                }
                sound.PlaySE(SoundManager.Sound.MissionStart);
                isFirst = false;
                isMission = true;
                missionMgr.mission[missionNum].state = MissionMgr.MissionState.PLAY;
                missionMgr.ui.MissionCenterImage(missionNum);                
                timeline.SetActive(true);
                stageMgr.gameState = StageMgr.GameState.Tips;
                return;
            }
            
            if (isFirst)
            {
                stageMgr.isMissionCenter = true;
                sound.PlaySE(SoundManager.Sound.MissionStart);
                other.GetComponent<TotoFace>().ChangeFace(0);
                if (missionNum == 8)
                {
                    other.GetComponent<TotoFace>().ChangeFace(2);
                }
                isFirst = false;
                isMission = true;
                missionMgr.mission[missionNum].state = MissionMgr.MissionState.PLAY;
                missionMgr.ui.MissionCenterImage(missionNum);
                for (int i = 0; i < spawnNPC.Length; ++i)
                {
                    spawnNPC[i].SetActive(true);
                }
            }
            else
            {
                if (isLast) return;
                if (missionNum == 4)
                {
                    other.GetComponent<TotoFace>().ChangeFace(3);
                    bgm.ChangeBGM(4);
                }
                if (missionNum == 8)
                {
                    other.GetComponent<TotoFace>().ChangeFace(2);
                    bgm.ChangeBGM(5);
                }
                else
                {
                    other.GetComponent<TotoFace>().ChangeFace(0);
                }
                missionMgr.ui.MissionBarImage(missionNum);
                if (/*missionNum == 4 || */missionNum == 6 || missionNum == 7)
                {
                    //spawnNPC[0].SetActive(true);
                    StartCoroutine(StartObject());
                    //if (spawnNPC[0].GetComponent<Enemy>())
                    //{
                    //    spawnNPC[0].GetComponent<Enemy>().SetMoveAnim();
                    //}
                }
            }          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isLast) return;
            missionMgr.ui.MissionBarOut();
            if (!bgm.CheckBgm(3))
            {
                bgm.ChangeBGM(3);
            }            
            if (/*missionNum == 4 || */missionNum == 6 || missionNum == 7)
            {
                //spawnNPC[0].SetActive(false);
                StartCoroutine(StopObject());
            }
        }
    }

    IEnumerator StopObject()
    {
        if(missionNum == 6)
        {
            SlopeMiniCar car = spawnNPC[0].GetComponent<SlopeMiniCar>();
            yield return null;
            if (car)
            {
                car.StopState();
                yield return null;
            }
            yield return null;
        }
        if (missionNum == 7)
        {
            TrainController train = spawnNPC[0].GetComponent<TrainController>();
            yield return null;
            if (train)
            {
                train.StopState();
                yield return null;
            }
            yield return null;
        }
        spawnNPC[0].SetActive(false);
        yield return null;
    }

    IEnumerator StartObject()
    {
        spawnNPC[0].SetActive(true);
        yield return null;
        if (missionNum == 6)
        {
            SlopeMiniCar car = spawnNPC[0].GetComponent<SlopeMiniCar>();
            yield return null;
            if (car)
            {
                car.Restart();
                yield return null;
            }
            yield return null;
        }
        if (missionNum == 7)
        {
            TrainController train = spawnNPC[0].GetComponent<TrainController>();
            yield return null;
            if (train)
            {
                train.Restart();
                yield return null;
            }
            yield return null;
        }
    }
}
