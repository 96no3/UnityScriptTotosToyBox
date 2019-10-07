using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StartUp
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        //カーソルの初期状態は表示状態
        Director.Instance.VisibleCursor(false);
    }
}

public class Director
{
    /*---------------シングルトン---------------*/
    //共有インスタンス
    private static Director m_Instance;    

    //共有インスタンスの取得
    public static Director Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new Director();
            }

            return m_Instance;
        }
    }
    /*---------------シングルトン---------------*/
        
    /**
     * @desc    スローモーションの開始
     * @param   通常速度からの倍率
     * @tips    1が通常速度
     *          0.1で10分の１の速度
     */
    public void StartSlowMotion(float scale)
    {
        Time.timeScale = scale;
    }

    /**
     * @desc    スローモーションの終了
     */
    public void StopSlowMotion()
    {
        Time.timeScale = 1.0f;
    }

    /**
     * @desc    カーソルの表示・非表示
     * @param   表示・非表示のフラグ
     */
    public void VisibleCursor(bool flag)
    {
        Cursor.visible = flag;

        if (flag)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /**
     * @desc    エディター or アプリケーションの終了
     */
    public void EndGame()
    {
#if UNITY_EDITOR            //エディターからの起動時
        EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE      //アプリケーションからの起動時
            Application.Quit();

#endif
    }
}

public class GameInstance
{

    /*---------------シングルトン---------------*/
    //共有インスタンス
    private static GameInstance m_Instance;

    //コンストラクタ
    private GameInstance() {
        clearTime = 0;
        hasKey = false;
        helpedNpcList = new List<NPC>();
        goalNpcList = new List<NPC>();
        coinList = new List<Coin>();
        spawnPosList = new List<Vector3>();        
    }

    //共有インスタンスの取得
    public static GameInstance Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameInstance();
            }

            return m_Instance;
        }
    }
    /*---------------シングルトン---------------*/
    public bool hasKey;
    public float clearTime;
    public List<NPC> helpedNpcList;
    public List<NPC> goalNpcList;
    public List<Coin> coinList;
    public List<Vector3> spawnPosList;    

    public void  Init()
    {
        clearTime = 0;
        hasKey = false;
        helpedNpcList = new List<NPC>();
        goalNpcList = new List<NPC>();
        coinList = new List<Coin>();
        spawnPosList = new List<Vector3>();
    }
}