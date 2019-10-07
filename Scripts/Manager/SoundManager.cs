using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    #region Singleton

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    Debug.LogError(typeof(SoundManager) + "is nothing");
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    public AudioClip nextSE,
                     cancelSE,
                     restartSE,
                     exitSE,
                     resultSE,
                     clearSE,
                     missionStartSE,
                     missionClearSE,
                     spawnPaperSE,
                     pythagoraSE,
                     goalSE,
                     fallBookSE,
                     trampolineSE,
                     swichSE,
                     aSE,
                     selectSE;

    public enum Sound
    {
        NEXT,
        CANCEL,
        RESTART,
        SELECT,
        RESULT,
        CLEAR,
        MissionStart,
        MissionClear,
        SpawnPaper,
        Pythagora,
        Goal,
        Swich,
        Trampoline,
        FallBook,
        A,
        EXIT
    }
    [HideInInspector] public Sound seType;

    AudioSource aud;
    [SerializeField] private AudioSource aud2;
    [SerializeField] private AudioSource aud3;

    [HideInInspector] public string horizontalInput = "Player1_Horizontal";
    //private bool buttonDown = false;
    private float time = 0;
    private float preInput = 0;

    public void InitInput()
    {
        preInput = 0;
        time = 0;
    }

    public void Awake()
    {
        //シングルトンのためのコード
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        aud = GetComponent<AudioSource>();
        preInput = 0;
        time = 0;
        //buttonDown = false;
    }

    public void SoundUpdate()
    {
        //if(time >= 0.55f)
        //{
        //    InitInput();
        //}

        //time += Time.deltaTime;
        //float input = Input.GetAxis(horizontalInput);        

        //if(Mathf.Abs(preInput-input) > 0.9f)
        //{
        //    aud.PlayOneShot(selectSE);
        //    time = 0;
        //}

        //preInput = input;


        //if (input > 1.0f || input < -1.0f)
        //{
        //    buttonDown = true;
        //}

        //if (buttonDown)
        //{
        //    time += Time.deltaTime;
        //}

        if (Input.GetButtonDown(horizontalInput))
        {
            aud.PlayOneShot(selectSE);
            //buttonDown = false;
            time = 0;
        }
    }

    public void PlaySE(Sound type)
    {
        switch (type)
        {
            case Sound.NEXT:
                aud.PlayOneShot(nextSE);
                break;
            case Sound.CANCEL:
                aud.PlayOneShot(cancelSE);
                break;
            case Sound.RESTART:
                aud.PlayOneShot(restartSE);
                break;
            case Sound.SELECT:
                aud.PlayOneShot(selectSE);
                break;
            case Sound.EXIT:
                aud.PlayOneShot(exitSE);
                break;
            case Sound.RESULT:
                aud3.PlayOneShot(resultSE);
                break;
            case Sound.MissionStart:
                aud.PlayOneShot(missionStartSE);
                break;
            case Sound.MissionClear:
                aud.PlayOneShot(missionClearSE);
                break;
            case Sound.SpawnPaper:
                aud3.PlayOneShot(spawnPaperSE);
                break;
            case Sound.Pythagora:
                aud.PlayOneShot(pythagoraSE);
                break;
            case Sound.FallBook:
                aud2.PlayOneShot(fallBookSE);
                break;
            case Sound.Trampoline:
                aud.PlayOneShot(trampolineSE);
                break;
            case Sound.Swich:
                aud3.PlayOneShot(swichSE);
                break;
            case Sound.Goal:
                aud.PlayOneShot(goalSE);
                break;
            case Sound.CLEAR:
                aud.PlayOneShot(clearSE);
                break;
            case Sound.A:
                aud.PlayOneShot(aSE);
                break;
            default:
                break;
        }
    }
}
