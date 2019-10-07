using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [SerializeField]
    Fade fadeImage;

    private SoundManager sound;
    private BGMMgr bgm;

    private bool onButton = false;

    private void Start()
    {        
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        onButton = false;
        bgm.PlayBGM(8);
        //Director.Instance.VisibleCursor(true); // デバック用
    }

    void Update()
    {
        if (Input.GetButtonDown("Player1_Esc"))
        {
            sound.PlaySE(SoundManager.Sound.EXIT);
            fadeImage.startFadeOut(SceneNamaes.Scenes.EXIT);
            return;
        }
    
        if (Input.GetButtonDown("Player1_Back"))
        {
            FinishCredit();
        }
    }

    public void FinishCredit()
    {
        if (onButton) return;

        onButton = true;
        sound.PlaySE(SoundManager.Sound.RESTART);
        fadeImage.startFadeOut(SceneNamaes.Scenes.TITLE);
    }

    public void StopBGM()
    {
        bgm.FadeOutBGM();
    }
}
