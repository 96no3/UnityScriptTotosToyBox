using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    private SoundManager sound;

    private Rigidbody rb;
    //外部公開パラメータ
    public GameObject[] goalCharactersPrefab;          ///< ゴールで得られるキャラクタのリスト
    private GameObject chara;
    private bool isGoal = false;

    public GameObject smokeEffect;
    private float time = 0;
    [SerializeField] private float finishTime = 2.0f;
    private Vector3 startPos;

    void Start()
    {
        isGoal = false;
        time = 0;
        startPos = transform.position;
        int index = Random.Range(0, goalCharactersPrefab.Length);
        chara = Instantiate(goalCharactersPrefab[index], startPos,Quaternion.identity);
        NPC npc = chara.GetComponent<NPC>();
        npc.SetId(100);
        chara.SetActive(false);
        smokeEffect.SetActive(false);        
        rb = GetComponent<Rigidbody>();
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (isGoal && time == 0)
        {
            CharaSpawn();
        }

        if (isGoal)
        {
            time += Time.deltaTime;
        }

        if (time > finishTime)
        {
            time = 0;
            smokeEffect.SetActive(false);
            gameObject.SetActive(false);            
        }
    }

    void CharaSpawn()
    {
        //キャラクタのスポーン処理
        chara.SetActive(true);        
        smokeEffect.transform.position = startPos;
        smokeEffect.SetActive(true);
        smokeEffect.transform.parent = null;
        transform.localScale = Vector3.zero;
        sound.PlaySE(SoundManager.Sound.SpawnPaper);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SoccerGoal")
        {
            isGoal = true;
            sound.PlaySE(SoundManager.Sound.Goal);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }
    }
}
