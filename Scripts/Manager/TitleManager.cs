using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    Fade fadeImage;

    private SoundManager sound;
    private BGMMgr bgm;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject skipCanvas;
    private float time = 0;
    private float movieTime = 50.0f;
    private bool isTitle = false;

    private void Start()
    {
        time = 0;
        isTitle = false;
        canvas.SetActive(false);
        skipCanvas.SetActive(true);
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        bgm.PlayBGM(0);
    }

    private void Update()
    {
        if (!isTitle)
        {
            if (Input.GetButtonDown("Player1_Back"))
            {
                sound.PlaySE(SoundManager.Sound.NEXT);
                StartTitle();
            }
        }        

        if (!canvas.activeInHierarchy)
        {
            time += Time.deltaTime;
        }

        if (time > movieTime)
        {
            StartTitle();
        }

        if (canvas.activeInHierarchy)
        {
            if (Input.GetButtonDown("Player1_Esc"))
            {
                sound.PlaySE(SoundManager.Sound.EXIT);
                fadeImage.startFadeOut(SceneNamaes.Scenes.EXIT);
                return;
            }

            sound.SoundUpdate();
        }
    }

    void StartTitle()
    {
        canvas.SetActive(true);
        skipCanvas.SetActive(false);
        time = 0;
        isTitle = true;
    }

    public void onClickPlay()
    {
        sound.PlaySE(SoundManager.Sound.NEXT);
        fadeImage.startFadeOut(SceneNamaes.Scenes.LOAD);
    }

    public void onClickCredit()
    {
        sound.PlaySE(SoundManager.Sound.NEXT);
        fadeImage.startFadeOut(SceneNamaes.Scenes.CREDIT);
    }

    public void onClickExit()
    {
        sound.PlaySE(SoundManager.Sound.EXIT);
        fadeImage.startFadeOut(SceneNamaes.Scenes.EXIT);
    }
}
