using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public MissionMgr missionMgr;

    [SerializeField] private Animator missionClearAnim;
    [SerializeField] private Animator keyAnim;
    [SerializeField] private Animator centerAnim;
    [SerializeField] private Animator missionCenterAnim;
    [SerializeField] private Animator helpedNumAnim;
    [SerializeField] private Animator missionBarAnim;
    [SerializeField] private Animator missionBarAnim2;
    //[SerializeField] private Animator checkKeyAnim;
    [SerializeField] private Animator check0Anim;
    [SerializeField] private Animator check1Anim;
    [SerializeField] private Animator check2Anim;
    [SerializeField] private Animator check3Anim;
    [SerializeField] private Animator check4Anim;
    [SerializeField] private Animator check5Anim;
    [SerializeField] private Animator check6Anim;

    [Header("UI")]
    [SerializeField] private Image missionClearImage;
    [SerializeField] private Image missionImage;   // missionCenterAnim用
    [SerializeField] private Image missionBarImage;
    [SerializeField] private Image[] missionIconImage;
    [SerializeField] private Image[] checkImage;
    [SerializeField] private Image npcCharaImage;  // HelpedNumAnim用
    [SerializeField] private Image[] npcNumImage;

    [SerializeField] private Sprite[] missionSprite;
    [SerializeField] private Sprite[] missionIconSprite;
    [SerializeField] private Sprite[] npcNumSprite;
    [SerializeField] private Sprite[] npcCharaSprite;
    [SerializeField] private Sprite[] missionClearSprite;

    private int currentNpcNum = 0;
    private int beforeNpcNum = 0;
    [HideInInspector] public int currentMissionNum = 0;

    [Header("Pause")]
    [SerializeField] private Image goalMissionBarImage;

    private void Start()
    {
        StartCoroutine("Init");
    }

    IEnumerator Init()
    {
        currentNpcNum = 0;
        beforeNpcNum = 0;
        currentMissionNum = 0;
        yield return null;
        SetHelpedNum();
        yield return null;
    }

    public void MissionClearAnim(int n)
    {
        missionClearImage.sprite = missionClearSprite[n];
        missionClearAnim.SetTrigger("Clear");
    }

    public void CenterImageStart()
    {
        centerAnim.SetTrigger("Start");
    }

    public void CenterImageFinish()
    {
        centerAnim.SetTrigger("Finish");
    }

    public void MissionCenterImage(int missionNo)
    {
        currentMissionNum = missionNo;
        missionImage.sprite = missionSprite[missionNo];
        missionCenterAnim.SetTrigger("MissionCenter");
    }

    public void MissionBarImage(int missionNo)
    {
        if (missionNo == 0)
        {
            missionBarAnim.SetTrigger("BarIn");
            return;
        }

        StartCoroutine("MissionBarSet", missionNo);
    }

    public void MissionBarOut()
    {
        missionBarAnim2.SetTrigger("BarOut");
    }

    //public void CheckKey()
    //{
    //    checkKeyAnim.SetTrigger("Check");
    //}

    public void CheckMission(int posNum)
    {
        switch (posNum)
        {
            case 0:
                check0Anim.SetTrigger("Check");
                break;
            case 1:
                check1Anim.SetTrigger("Check");
                break;
            case 2:
                check2Anim.SetTrigger("Check");
                break;
            case 3:
                check3Anim.SetTrigger("Check");
                break;
            case 4:
                check4Anim.SetTrigger("Check");
                break;
            case 5:
                check5Anim.SetTrigger("Check");
                break;
            case 6:
                check6Anim.SetTrigger("Check");
                break;
        }
    }

    public void CheckReset(int posNum)
    {
        switch (posNum)
        {
            case 0:
                check0Anim.SetTrigger("None");
                break;
            case 1:
                check1Anim.SetTrigger("None");
                break;
            case 2:
                check2Anim.SetTrigger("None");
                break;
            case 3:
                check3Anim.SetTrigger("None");
                break;
            case 4:
                check4Anim.SetTrigger("None");
                break;
            case 5:
                check5Anim.SetTrigger("None");
                break;
            case 6:
                check6Anim.SetTrigger("None");
                break;
        }
    }

    public void KeyImageIn()
    {
        keyAnim.SetTrigger("KeyIn");
    }

    public void KeyImageOut()
    {
        keyAnim.SetTrigger("KeyOut");
    }

    public void HelpedNumCarHit()
    {
        currentNpcNum = GameInstance.Instance.helpedNpcList.Count;
        beforeNpcNum = currentNpcNum + 1;
        npcNumImage[0].sprite = npcNumSprite[beforeNpcNum];
        npcNumImage[1].sprite = npcNumSprite[currentNpcNum];
        helpedNumAnim.SetTrigger("CarHit");
    }

    public void HelpedNumEnemyHit()
    {
        currentNpcNum = GameInstance.Instance.helpedNpcList.Count;
        beforeNpcNum = currentNpcNum + 1;
        npcNumImage[0].sprite = npcNumSprite[beforeNpcNum];
        npcNumImage[1].sprite = npcNumSprite[currentNpcNum];
        helpedNumAnim.SetTrigger("EnemyHit");
    }

    public void HelpedNumAddNpc()
    {
        int x = Random.Range(0, 2);
        npcCharaImage.sprite = npcCharaSprite[x];
        currentNpcNum = GameInstance.Instance.helpedNpcList.Count;
        beforeNpcNum = currentNpcNum - 1;
        npcNumImage[0].sprite = npcNumSprite[beforeNpcNum];
        npcNumImage[1].sprite = npcNumSprite[currentNpcNum];
        helpedNumAnim.SetTrigger("AddNpc");
    }

    public void SetHelpedNum()
    {
        int tempNum = GameInstance.Instance.helpedNpcList.Count;
        StartCoroutine(SetNum(tempNum));
    }

    public void EnableBar(bool flag,int n)
    {
        goalMissionBarImage.enabled = flag;
        missionBarImage.enabled = flag;

        for (int i = 0;i< missionIconImage.Length; i++)
        {
            missionIconImage[i].enabled = flag;
        }
        for (int i = 0; i < checkImage.Length; i++)
        {
            checkImage[i].sprite = missionIconSprite[n];
        }
    }

    IEnumerator SetNum(int n)
    {
        currentNpcNum = n;
        beforeNpcNum = n;
        npcNumImage[0].sprite = npcNumSprite[n];
        npcNumImage[1].sprite = npcNumSprite[n];
        yield return null;
    }

    IEnumerator MissionBarSet(int missionNo)
    {
        currentMissionNum = missionNo;
        missionBarImage.sprite = missionSprite[missionNo];

        for (int i = 0; i < 7; ++i)
        {
            missionIconImage[i].sprite = missionIconSprite[0];
            CheckReset(i);
            yield return null;
        }

        for (int i = 0; i < missionMgr.mission[missionNo].npcNum; ++i)
        {
            if (missionMgr.mission[missionNo].missionNpc[i].isCheck)
            {
                CheckMission(i);                
            }
            yield return null;
        }

        switch (missionNo)
        {
            case 1:
                missionIconImage[0].sprite = missionIconSprite[1];
                missionIconImage[1].sprite = missionIconSprite[2];
                missionIconImage[2].sprite = missionIconSprite[3];
                break;
            case 2:
                missionIconImage[0].sprite = missionIconSprite[4];
                missionIconImage[1].sprite = missionIconSprite[5];
                break;
            case 3:
                missionIconImage[0].sprite = missionIconSprite[19];
                break;
            case 4:
                missionIconImage[0].sprite = missionIconSprite[6];
                missionIconImage[1].sprite = missionIconSprite[7];
                missionIconImage[2].sprite = missionIconSprite[8];
                break;
            case 5:
                missionIconImage[0].sprite = missionIconSprite[20];
                missionIconImage[1].sprite = missionIconSprite[20];
                missionIconImage[2].sprite = missionIconSprite[20];
                missionIconImage[3].sprite = missionIconSprite[20];
                missionIconImage[4].sprite = missionIconSprite[20];
                break;
            case 6:
                missionIconImage[0].sprite = missionIconSprite[9];
                missionIconImage[1].sprite = missionIconSprite[10];
                missionIconImage[2].sprite = missionIconSprite[11];
                break;
            case 7:
                missionIconImage[0].sprite = missionIconSprite[12];
                missionIconImage[1].sprite = missionIconSprite[13];
                missionIconImage[2].sprite = missionIconSprite[14];
                missionIconImage[3].sprite = missionIconSprite[15];
                missionIconImage[4].sprite = missionIconSprite[16];
                missionIconImage[5].sprite = missionIconSprite[17];
                missionIconImage[6].sprite = missionIconSprite[18];
                break;
            case 8:
                missionIconImage[0].sprite = missionIconSprite[22];
                break;
            default:
                break;
        }
        yield return null;

        missionBarAnim2.SetTrigger("BarIn");
        yield return null;
    }
}
