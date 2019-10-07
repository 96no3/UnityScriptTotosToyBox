using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMgr : MonoBehaviour
{
    #region Singleton

    private static BGMMgr instance;

    public static BGMMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (BGMMgr)FindObjectOfType(typeof(BGMMgr));

                if (instance == null)
                {
                    Debug.LogError(typeof(BGMMgr) + "is nothing");
                }
            }

            return instance;
        }
    }

    #endregion Singleton

    public float fadeTime = 1.0f;

    [SerializeField] private AudioSource audio1;
    [SerializeField] private AudioSource audio2;

    private AudioSource[] _audios = new AudioSource[2];
    public AudioClip[] bgm;

    private float elapsed = 0.0f;
    private float bgmVolume = 1.0f;
    private bool isChange = false;
    private bool isFadeOut = false;

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
        audio1.Play();
        audio1.volume = 1.0f;
        audio2.Stop();
        audio2.volume = 0.0f;
        _audios[0] = audio1;
        _audios[1] = audio2;
        elapsed = 0.0f;
    }
        
    void Update()
    {
        if (isChange)
        {
            elapsed += Time.deltaTime;

            float rate = elapsed / fadeTime;

            _audios[0].volume = bgmVolume * (1.0f - rate);
            _audios[1].volume = bgmVolume * rate;

            if (elapsed > fadeTime)
            {
                _audios[0].volume = 0.0f;
                _audios[0].Stop();
                _audios[1].volume = bgmVolume;
                isChange = false;
                elapsed = 0.0f;
            }
        }

        if (isFadeOut)
        {
            elapsed += Time.deltaTime;

            float rate = elapsed / fadeTime;

            _audios[0].volume = bgmVolume * (1.0f - rate);
            _audios[1].volume = bgmVolume * (1.0f - rate);

            if (elapsed > fadeTime)
            {
                elapsed = 0.0f;
                StopBGM();
                isFadeOut = false;
            }
        }
    }

    public bool CheckBgm(int n)
    {
        if (audio1.isPlaying)
        {
            if (audio1.clip == bgm[n])
            {
                return true;
            }
        }

        if (audio2.isPlaying)
        {
            if (audio2.clip == bgm[n])
            {
                return true;
            }
        }

        return false;        
    }

    public void ChangeBGM(int n)
    {
        if (audio1.isPlaying && !audio2.isPlaying)
        {
            bgmVolume = audio1.volume;
            audio2.volume = 0.0f;
            audio2.clip = bgm[n];
            audio2.Play();
            _audios[0] = audio1;
            _audios[1] = audio2;
            isChange = true;
        }
        else if (!audio1.isPlaying && audio2.isPlaying)
        {
            bgmVolume = audio2.volume;
            audio1.volume = 0.0f;
            audio1.clip = bgm[n];
            audio1.Play();
            _audios[0] = audio2;
            _audios[1] = audio1;
            isChange = true;
        }
    }

    public void PlayBGM(int n)
    {
        StopBGM();
        audio1.volume = 1.0f;
        audio1.clip = bgm[n];
        audio1.Play();
    }

    public void StopBGM()
    {
        if (audio1.isPlaying)
        {
            audio1.Stop();
            audio1.volume = 0;
            audio1.clip = null;
        }
        if (audio2.isPlaying)
        {
            audio2.Stop();
            audio2.volume = 0;
            audio2.clip = null;
        }        
    }

    public void FadeOutBGM()
    {
        isFadeOut = true;
    }
}
