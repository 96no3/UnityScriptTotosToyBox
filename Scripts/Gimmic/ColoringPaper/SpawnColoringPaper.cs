using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnColoringPaper : MonoBehaviour
{
    private SoundManager sound;

    //外部公開パラメータ
    [SerializeField] private NPC chara;
    [HideInInspector] public bool onPlayer = false;
    private bool isSpawn = false;

    public GameObject smokeEffect;
    private float time = 0;
    private float diffTime = 0;
    [SerializeField] private float spawnTime = 3.0f;
    [SerializeField] private float finishTime = 2.0f;

    [HideInInspector] public bool isPause = false;

    void Start()
    {
        onPlayer = false;
        isSpawn = false;
        isPause = false;
        time = 0;
        diffTime = 0;
        chara.transform.parent = null;
        chara.gameObject.SetActive(false);
        smokeEffect.SetActive(false);
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (onPlayer && !isPause)
        {
            diffTime += Time.deltaTime;
        }

        if (diffTime >= spawnTime)
        {
            CharaSpawn();
            isSpawn = true;
            diffTime = 0;
        }

        if (isSpawn)
        {
            time += Time.deltaTime;
        }

        if (time > finishTime)
        {
            time = 0;
            smokeEffect.SetActive(false);
            transform.parent.gameObject.SetActive(false);
        }
    }

    void CharaSpawn()
    {
        //キャラクタのスポーン処理
        chara.gameObject.SetActive(true);
        chara.transform.position = transform.position;
        smokeEffect.SetActive(true);
        smokeEffect.transform.parent = null;
        transform.localScale = Vector3.zero;
        sound.PlaySE(SoundManager.Sound.SpawnPaper);
        chara.anim.SetTrigger("Hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            onPlayer = false;
        }
    }
}
