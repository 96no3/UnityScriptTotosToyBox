using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMgr : MonoBehaviour
{
    public UIMgr ui;

    public enum MissionState
    {
        NEW,
        PLAY,
        CLEAR
    }

    public struct MissionNPC
    {
        public int npcID;
        public bool isCheck;
    }

    public struct Mission
    {
        public int npcNum;
        public MissionState state;
        public MissionNPC[] missionNpc;
    }
    public Mission[] mission;

    void Start()
    {
        mission = new Mission[9];
        StartCoroutine("InitMissionFirst");
    }

    public bool CheckID(int missionNum, int id)
    {
        for (int i = 0; i < mission[missionNum].npcNum; ++i)
        {
            if(id == mission[missionNum].missionNpc[i].npcID)
            {
                if (!mission[missionNum].missionNpc[i].isCheck)
                {
                    mission[missionNum].missionNpc[i].isCheck = true;
                    ui.CheckMission(i);
                    return true;
                }                
            }
        }
        return false;
    }

    public bool CheckNpc(int missionNum)
    {
        for (int i = 0; i < mission[missionNum].npcNum; ++i)
        {
            if (mission[missionNum].missionNpc[i].isCheck == false)
            {
                return false;
            }
        }
        return true;
    }

    private void InitMission(int missionNum)
    {
        mission[missionNum].state = MissionState.NEW;

        if (missionNum == 0) return;        

        switch (missionNum)
        {
            case 1:
                mission[missionNum].npcNum = 3;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];
                int npc1Id = 1;
                for (int i = 0; i < mission[missionNum].npcNum; ++i)
                {
                    mission[missionNum].missionNpc[i].npcID = npc1Id;
                    mission[missionNum].missionNpc[i].isCheck = false;
                    npc1Id++;
                }
                break;
            case 2:
                mission[missionNum].npcNum = 2;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];
                int npc2Id = 4;
                for (int i = 0; i < mission[missionNum].npcNum; ++i)
                {
                    mission[missionNum].missionNpc[i].npcID = npc2Id;
                    mission[missionNum].missionNpc[i].isCheck = false;
                    npc2Id++;
                }
                break;
            case 3:
                mission[missionNum].npcNum = 1;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];
                mission[missionNum].missionNpc[0].npcID = 100;
                mission[missionNum].missionNpc[0].isCheck = false;
                break;
            case 4:
                mission[missionNum].npcNum = 3;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];
                int npc4Id = 6;
                for (int i = 0; i < mission[missionNum].npcNum; ++i)
                {
                    mission[missionNum].missionNpc[i].npcID = npc4Id;
                    mission[missionNum].missionNpc[i].isCheck = false;
                    npc4Id++;
                }                
                break;
            case 5:
                mission[missionNum].npcNum = 5;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];

                for (int i = 0; i < mission[missionNum].npcNum; ++i)
                {
                    mission[missionNum].missionNpc[i].npcID = 200;
                    mission[missionNum].missionNpc[i].isCheck = false;
                }
                break;
            case 6:
                int npc6Id = 9;
                mission[missionNum].npcNum = 3;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];

                for (int i = 0; i < mission[missionNum].npcNum; ++i)
                {
                    mission[missionNum].missionNpc[i].npcID = npc6Id;
                    mission[missionNum].missionNpc[i].isCheck = false;
                    npc6Id++;
                }
                break;
            case 7:
                mission[missionNum].npcNum = 7;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];
                int npc7Id = 12;
                for (int i = 0; i < mission[missionNum].npcNum; ++i)
                {
                    mission[missionNum].missionNpc[i].npcID = npc7Id;
                    mission[missionNum].missionNpc[i].isCheck = false;
                    npc7Id++;
                }
                break;
            case 8:
                mission[missionNum].npcNum = 1;
                mission[missionNum].missionNpc = new MissionNPC[mission[missionNum].npcNum];
                mission[missionNum].missionNpc[0].npcID = 900;
                mission[missionNum].missionNpc[0].isCheck = false;
                break;
        }
    }

    IEnumerator InitMissionFirst()
    {
        for (int i = 0; i < 9; ++i)
        {
            InitMission(i);
            yield return null;
        }
        yield return null;
    }
}
