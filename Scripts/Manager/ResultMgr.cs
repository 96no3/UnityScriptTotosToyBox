using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMgr : MonoBehaviour
{    
    [HideInInspector] public ResultUIMgr ui;
    private ResultNpcMgr resultNpcMgr;
    private SoundManager sound;
    private BGMMgr bgm;

    [SerializeField] private RankingScript ranking;
    [SerializeField] private Fade fadeImage;
    [SerializeField] private LifePanel starPanel;
    [SerializeField] private TotoFace face;
    [SerializeField] private GameObject resultCanvas;

    [SerializeField] private Animator rankingCanvasAnim;
    [SerializeField] private Animator totoAnim;

    [SerializeField] private Text timeText;

    private bool isRanking = false;
    int starNum = 0;

    private float count = 0;

    void Start()
    {
        StartCoroutine("Init");
    }

    private void Update()
    {
        if (resultCanvas.activeInHierarchy)
        {
            if (Input.GetButtonDown("Player1_Esc"))
            {
                sound.PlaySE(SoundManager.Sound.EXIT);
                fadeImage.startFadeOut(SceneNamaes.Scenes.EXIT);
                return;
            }

            sound.SoundUpdate();
        }

        if (isRanking)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ResultButtonDown();
                isRanking = false;
            }

            if (Input.GetButton("Player1_B"))
            {
                count += Time.deltaTime;
                if (count > 10.0f)
                {
                    count = 0;
                    ranking.ResetRanking();
                    sound.PlaySE(SoundManager.Sound.RESTART);
                }                
            }
        }
    }

    private string GetTimeString(float time)
    {
        string str = ConvertMinite(time).ToString("D2") + " : " + ConvertSecond(time).ToString("D2");
        return str;
    }

    private int ConvertMinite(float time)
    {
        int m = (int)time / 60;
        return m;
    }

    private int ConvertSecond(float time)
    {
        int s = (int)time % 60;
        return s;
    }

    private int Star(int rescuer)
    {
        if (rescuer >= 20)
        {
            face.ChangeFace(3);
            return 3;
        }
        else if (rescuer >= 9)
        {
            face.ChangeFace(2);
            return 2;
        }
        else if (rescuer > 0)
        {
            face.ChangeFace(1);
            return 1;
        }
        else
        {
            face.ChangeFace(0);
            return 0;
        }
    }

    public void TitleButtonDown()
    {
        sound.PlaySE(SoundManager.Sound.RESTART);
        fadeImage.startFadeOut(SceneNamaes.Scenes.TITLE);
    }

    public void CharaButtonDown()
    {
        sound.PlaySE(SoundManager.Sound.NEXT);
        fadeImage.startFadeOut(SceneNamaes.Scenes.CHARACTER);
    }

    public void RankingButtonDown()
    {
        sound.PlaySE(SoundManager.Sound.A);
        rankingCanvasAnim.SetTrigger("In");
    }

    public void ResultButtonDown()
    {
        sound.PlaySE(SoundManager.Sound.CANCEL);
        resultCanvas.SetActive(true);
        rankingCanvasAnim.SetTrigger("Out");
    }

    public void SetRanking()
    {
        isRanking = true;
        resultCanvas.SetActive(false);
    }

    public void SetResult()
    {
        ResetAnimation();
    }

    public void ResetAnimation()
    {
        StartCoroutine("ResetAnim");
    }

    IEnumerator Init()
    {
        starNum = Star(GameInstance.Instance.goalNpcList.Count);
        ui = GetComponent<ResultUIMgr>();
        resultNpcMgr = GetComponent<ResultNpcMgr>();
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        yield return null;        
        bgm.PlayBGM(6);
        isRanking = false;
        resultCanvas.SetActive(true);
        timeText.text = GetTimeString(GameInstance.Instance.clearTime);
        yield return null;
        sound.PlaySE(SoundManager.Sound.RESULT);
        totoAnim.SetTrigger("Clear");
        face.ChangeFace(starNum);
        yield return null;
        // ランクパネルを更新
        starPanel.UpdateLife(starNum);
        ui.SetHelpedNumAnim();
        ui.MissionClearAnim(starNum);
        yield return null;
        resultNpcMgr.InitResultNpc();
        yield return null;
    }

    IEnumerator ResetAnim()
    {
        starNum = Star(GameInstance.Instance.goalNpcList.Count);
        sound.PlaySE(SoundManager.Sound.RESULT);
        totoAnim.SetTrigger("Clear");
        face.ChangeFace(starNum);
        yield return null;
        // ランクパネルを更新
        starPanel.UpdateLife(starNum);
        ui.SetHelpedNumAnim();
        ui.MissionClearAnim(starNum);
        yield return null;
        resultNpcMgr.InitResultNpc();
        yield return null;
    }
}
