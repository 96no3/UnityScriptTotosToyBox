using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingMgr : MonoBehaviour {

    [SerializeField] private Fade fadeImage;
    [SerializeField] private Animator skipAnim;

    private SoundManager sound;
    private BGMMgr bgm;

     private AsyncOperation m_Async;
    //ロード完了後、次のシーンへ移るまでの待機時間
    [SerializeField] private float WaitTimeAfterLoadCompletion = 7.0f;

    private bool onButton = false;
    private bool isLoadFinish = false;
    private float diffTime = 0;

    [Header("Input")]
    public string escInput = "Player1_Esc";
    public string backInput = "Player1_Back"; // ムービースキップ用

    /**
     * @desc    初期化
     */
    void Start()
    {
        onButton = false;
        isLoadFinish = false;
        diffTime = 0;
        //Director.Instance.VisibleCursor(true); // デバック用
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        bgm.PlayBGM(1);
    }

    private void Update()
    {
        diffTime += Time.deltaTime;

        if (Input.GetButtonDown(escInput))
        {
            sound.PlaySE(SoundManager.Sound.EXIT);
            fadeImage.startFadeOut(SceneNamaes.Scenes.EXIT);
            return;
        }

        if(!isLoadFinish && diffTime > WaitTimeAfterLoadCompletion)
        {
            isLoadFinish = true;
            skipAnim.SetTrigger("Skip");            
        }

        if (!isLoadFinish) return;

        if (Input.GetButtonDown(backInput) && !onButton)
        {
            onButton = true;
            sound.PlaySE(SoundManager.Sound.NEXT);
            fadeImage.startFadeOut(SceneNamaes.Scenes.GAME);
            //コルーチン開始
            //StartCoroutine(LoadScene());
        }

        if (isLoadFinish && diffTime > (WaitTimeAfterLoadCompletion * 2))
        {
            diffTime = 0;
            isLoadFinish = false;
            sound.PlaySE(SoundManager.Sound.NEXT);
            fadeImage.startFadeOut(SceneNamaes.Scenes.GAME);
        }
    }

    ///**
    // * @desc    ロードが完了するまでの処理
    // * @tips    コルーチン
    // */
    //IEnumerator LoadScene()
    //{
    //    //裏で次のシーンを読み込ませて、その情報を取得
    //    m_Async = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
    //    //m_Async.allowSceneActivation = false;

    //    //ロードが完了していなければ
    //    while (m_Async.progress < 0.9f)
    //    {
    //        Debug.Log(m_Async.progress);
    //        //次のフレームに移行
    //        yield return null;
    //    }
    //    m_Async.allowSceneActivation = true;
    //    yield return null;
    //}
}
