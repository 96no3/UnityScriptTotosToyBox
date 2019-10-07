using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StageMgr : MonoBehaviour
{
    [SerializeField] private GameObject train;
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject[] eggs;
    [SerializeField] private GameObject[] bones;
    [SerializeField] private GameObject[] monkeys;
    [SerializeField] private GameObject[] coloringPapers;

    [SerializeField] private Fade fadeImage;
    [SerializeField] private GameObject PauseScene;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private IntroUiMgr IntroScene;
    
    public UIMgr ui;
    public MissionMgr missionMgr;
    [HideInInspector] public SoundManager sound;
    [HideInInspector] public BGMMgr bgm;

    public enum GameState
    {
        Intro,
        Tips,
        Run,
        Pause,
        Event,
        Clear,
        End
    }
    [HideInInspector] public GameState gameState;

    [Header("Parameter")]
    public float eventTime = 7.0f;
    [SerializeField] private float clearEventTime = 5.0f;

    public void SetEventTime(float num)
    {
        eventTime = num;
    }

    [HideInInspector] public float time = 0;
    [HideInInspector] public bool isAddGoal = false;    

    [Header("Objects")]
    private GameObject playerObject;
    private ThirdPersonCamera cameraObject;    // メインカメラへの参照
    [HideInInspector] public Player player;
    private Rigidbody rbPlayer;

    [Header("Input")]
    public string pauseInput = "Player1_Pause";
    public string backInput = "Player1_Back"; // ムービースキップ用
    public string escInput = "Player1_Esc";

    private bool isMission = false;
    [HideInInspector] public bool isMissionClear = false;
    [HideInInspector] public bool isStart = true;
    [HideInInspector] public bool isTimelineEnd = false;
    private float missionAnimTime = 2.30f;

    [HideInInspector] public bool isMissionCenter = false;

    private void Awake()
    {
        GameInstance.Instance.Init();
        //Director.Instance.VisibleCursor(true); // デバック用
    }

    void Start()
    {
        isMissionCenter = false;
        isStart = true;
        isTimelineEnd = false;
        isMission = false;
        isMissionClear = false;
        time = 0;
        isAddGoal = false;
        gameState = GameState.Tips;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        player = playerObject.GetComponent<Player>();
        rbPlayer = playerObject.GetComponent<Rigidbody>();
        //メインカメラを取得する
        cameraObject = GameObject.FindWithTag("MainCamera").GetComponent<ThirdPersonCamera>();
        PauseScene.SetActive(false);
        IntroScene.gameObject.SetActive(false);
        gameCanvas.SetActive(false);
        bgm.PlayBGM(2);
    }

    void Update()
    {
        if (Input.GetButtonDown(escInput))
        {
            sound.PlaySE(SoundManager.Sound.EXIT);
            fadeImage.startFadeOut(SceneNamaes.Scenes.EXIT);
            return;
        }

        if (isAddGoal)
        {
            if (GameInstance.Instance.helpedNpcList.Count <= 0)
            {
                isAddGoal = false;
            }
            else
            {
                List<NPC> tmpList = new List<NPC>();
                foreach (NPC e in GameInstance.Instance.helpedNpcList)
                {
                    tmpList.Add(e);
                }
                foreach (NPC e in tmpList)
                {
                    GameInstance.Instance.helpedNpcList.Remove(e);
                    GameInstance.Instance.goalNpcList.Add(e);
                    e.npcState = NPC.NPCState.Rescued;
                }
            }
        }

        switch (gameState)
        {
            case GameState.Intro:
                UpdateIntro();
                break;
            case GameState.Tips:
                UpdateTips();
                break;
            case GameState.Run:
                UpdateRun();
                break;
            case GameState.Pause:
                UpdatePause();
                break;
            case GameState.Event:
                UpdateEvent();
                break;
            case GameState.Clear:
                UpdateClear();
                break;
            case GameState.End:
                UpdateEnd();
                break;
            default:
                break;
        }
    }

    private void UpdateIntro()
    {
        if(time == 0)
        {
            IntroScene.gameObject.SetActive(true);
            IntroScene.StartMessage();
        }
        time = 0.0001f;
    }

    private void UpdateTips()
    {
        if(player.playerState == Player.PlayerState.Run)
        {
            player.playerState = Player.PlayerState.Stay;
        }
        //SkipMove();
        if (isStart && isTimelineEnd)
        {
            isStart = false;
            isTimelineEnd = false;
            gameState = GameState.Intro;
        }
        if (isTimelineEnd)
        {
            isTimelineEnd = false;
            player.playerState = Player.PlayerState.Run;
            gameState = GameState.Run;
        }
    }
        
    private void UpdateRun()
    {
        GameInstance.Instance.clearTime += Time.deltaTime;

        if (isMission)
        {            
            time += Time.deltaTime;
            if (time > missionAnimTime)
            {                
                player.playerState = Player.PlayerState.Run;
                time = 0;
                isMission = false;
                ui.MissionBarImage(0);
            }
        }

        if (isMissionClear)
        {
            isMissionClear = false;
            player.playerState = Player.PlayerState.Clear;
        }

        if (isMissionCenter) return;

        if (Input.GetButtonDown(pauseInput))
        {
            SetPauseScene();
            return;
        }        
    }

    private void UpdatePause()
    {
        if (Input.GetButtonDown(pauseInput))
        {
            StartCoroutine(OnPauseReset());
        }
    }

    public void SetPauseScene()
    {
        sound.PlaySE(SoundManager.Sound.SELECT);
        gameState = GameState.Pause;
        StartCoroutine(SetPause());
    }

    private void UpdateEvent()
    {
        player.playerState = Player.PlayerState.Stay;
        time += Time.deltaTime;

        if (time >= eventTime)
        {
            cameraObject.isEvent = false;
            player.playerState = Player.PlayerState.Run;
            time = 0;
            gameState = GameState.Run;
        }
    }

    private void UpdateClear()
    {
        if (time == 0)
        {
            sound.PlaySE(SoundManager.Sound.CLEAR);
        }
        
        time += Time.deltaTime;

        if (time > clearEventTime)
        {
            time = 0;
            gameState = StageMgr.GameState.End;            
        }
    }

    private void UpdateEnd()
    {
        if(time == 0)
        {
            ui.CenterImageFinish();
            time = -1;
        }        
    }    

    IEnumerator SetPause()
    {
        PauseScene.SetActive(true);
        ui.EnableBar(false, 0); // 0はアルファ0のスプライト
        yield return null;

        player.playerState = Player.PlayerState.Stay;
        if (rbPlayer.useGravity)
        {
            rbPlayer.useGravity = false;
        }
        yield return null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            foreach (GameObject e in enemies)
            {
                e.GetComponent<Enemy>().enemyState = Enemy.EnemyState.None;
            }
        }
        yield return null;

        foreach (GameObject e in coloringPapers)
        {
            SpawnColoringPaper trigger = e.GetComponent<SpawnColoringPaper>();
            if (trigger.onPlayer)
            {
                trigger.isPause = true;
            }
        }
        yield return null;

        foreach(GameObject e in eggs)
        {
            if (e.activeInHierarchy)
            {
                Egg egg = e.GetComponent<Egg>();
                if (egg.isActive)
                {
                    egg.npcState = NPC.NPCState.None;
                }
            }
        }
        yield return null;

        foreach (GameObject e in bones)
        {
            if (e.activeInHierarchy)
            {
                Bone bone = e.GetComponent<Bone>();
                if (bone.isActive)
                {
                    bone.npcState = NPC.NPCState.None;
                }
            }
        }
        yield return null;

        foreach (GameObject e in monkeys)
        {
            if (e.activeInHierarchy)
            {
                Monkey monkey = e.GetComponent<Monkey>();
                if (monkey.isActive)
                {
                    monkey.npcState = NPC.NPCState.None;
                }
            }
        }
        yield return null;
        
        if (car.activeInHierarchy)
        {
            car.GetComponent<SlopeMiniCar>().state = SlopeMiniCar.State.None;
        }
        yield return null;

        if (train.activeInHierarchy)
        {
            train.GetComponent<TrainController>().state = TrainController.State.None;
        }
        yield return null;

        //Director.Instance.StartSlowMotion(0);
    }

    IEnumerator OnPauseReset()
    {
        sound.PlaySE(SoundManager.Sound.CANCEL);
        //Director.Instance.StopSlowMotion();
        player.playerState = Player.PlayerState.Run;
        if (!rbPlayer.useGravity && !player.onUFO && !player.onUp)
        {
            rbPlayer.useGravity = true;
        }
        yield return null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            foreach (GameObject e in enemies)
            {
                e.GetComponent<Enemy>().enemyState = Enemy.EnemyState.Move;
            }
        }
        yield return null;

        foreach (GameObject e in coloringPapers)
        {
            SpawnColoringPaper trigger = e.GetComponent<SpawnColoringPaper>();
            if (trigger.onPlayer)
            {
                trigger.isPause = false;
            }
        }
        yield return null;

        foreach (GameObject e in eggs)
        {
            if (e.activeInHierarchy)
            {
                Egg egg = e.GetComponent<Egg>();
                if (egg.isActive)
                {
                    egg.npcState = NPC.NPCState.Help;
                }
            }
        }
        yield return null;

        foreach (GameObject e in bones)
        {
            if (e.activeInHierarchy)
            {
                Bone bone = e.GetComponent<Bone>();
                if (bone.isActive)
                {
                    bone.npcState = NPC.NPCState.Help;
                }
            }
        }
        yield return null;

        foreach (GameObject e in monkeys)
        {
            if (e.activeInHierarchy)
            {
                Monkey monkey = e.GetComponent<Monkey>();
                if (monkey.isActive)
                {
                    monkey.npcState = NPC.NPCState.Help;
                }
            }
        }
        yield return null;

        if (car.activeInHierarchy)
        {
            car.GetComponent<SlopeMiniCar>().Restart();
        }
        yield return null;

        if (train.activeInHierarchy)
        {
            train.GetComponent<TrainController>().Restart();
        }
        yield return null;        

        PauseScene.SetActive(false);
        ui.EnableBar(true,21); // 21はチェックのスプライト
        yield return null;

        gameState = GameState.Run;
        yield return null;
    }

    public void OnTitleButton()
    {
        sound.PlaySE(SoundManager.Sound.RESTART);
        fadeImage.startFadeOut(SceneNamaes.Scenes.TITLE);
    }

    public void GameStart()
    {        
        ui.MissionCenterImage(0);
        sound.PlaySE(SoundManager.Sound.MissionStart);
        //cameraObject.isEvent = false;        
        time = 0;
        isMission = true;
        gameState = GameState.Run;
        player.ResetPlayer();
    }

    public void GameEnd()
    {
        fadeImage.startFadeOut(SceneNamaes.Scenes.RESULT);
    }

    //private void SkipMove()
    //{
    //    if (Input.GetButtonDown(backInput))
    //    {
    //        sound.PlaySE(SoundManager.Sound.CANCEL);
    //        fadeImage.FadeOut();
    //        GameStart();
    //    }        
    //}
}
