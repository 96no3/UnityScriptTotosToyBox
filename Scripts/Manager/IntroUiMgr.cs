using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUiMgr : MonoBehaviour
{
    private BGMMgr bgm;

    [HideInInspector] public SoundManager sound;

    [SerializeField] private UIMgr ui;
    [SerializeField] private GameObject gameCanvas;

    [SerializeField] private Animator messageButtonAnim;

    [Header("UI")]
    [SerializeField] private Image messageButtonImage;
    [SerializeField] private Sprite[] messageSprite;

    private int currentMessageNum = 0;

    public void StartMessage()
    {
        bgm = GameObject.FindGameObjectWithTag("BGMMgr").GetComponent<BGMMgr>();
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        sound.PlaySE(SoundManager.Sound.A);
        messageButtonImage.sprite = messageSprite[currentMessageNum];
        messageButtonAnim.SetTrigger("In");
        currentMessageNum++;
    }

    public void OnMessageButton()
    {
        if(currentMessageNum == messageSprite.Length)
        {
            bgm.ChangeBGM(3);
            sound.PlaySE(SoundManager.Sound.NEXT);
            currentMessageNum = 0;
            gameCanvas.SetActive(true);
            ui.CenterImageStart();
            gameObject.SetActive(false);
        }
        else
        {
            StartMessage();
        }
    }
}
