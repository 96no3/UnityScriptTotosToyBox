using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gachapon : MonoBehaviour
{
    private SoundManager sound;

    //外部公開パラメータ
    public GameObject[] gachaCharactersPrefab;          ///< ガチャで得られるキャラクタのリスト
    private GameObject chara;
    private bool isLanding = false;    

    public GameObject smokeEffect;
    private float time = 0;
    [SerializeField] private float finishTime = 2.0f;

    void Start()
    {        
        isLanding = false;
        time = 0;
        int index = Random.Range(0, gachaCharactersPrefab.Length);
        chara = Instantiate(gachaCharactersPrefab[index]);
        NPC n = chara.GetComponent<NPC>();
        n.SetId(200);
        chara.SetActive(false);
        smokeEffect.SetActive(false);
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    private void Update()
    {        
        if (isLanding && time == 0)
        {
            CharaSpawn();
        }

        if (isLanding)
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
        //ガチャ結果のキャラクタのスポーン処理        
        List<Vector3> tmpList = new List<Vector3>();
        foreach (Vector3 e in GameInstance.Instance.spawnPosList)
        {
            tmpList.Add(e);
            break;
        }

        foreach (Vector3 e in tmpList)
        {            
            chara.transform.position = e;
            chara.SetActive(true);
            smokeEffect.transform.parent = null;
            smokeEffect.transform.position = e;
            smokeEffect.SetActive(true);
            GameInstance.Instance.spawnPosList.Remove(e);
            break;
        }               
        transform.localScale = Vector3.zero;
        sound.PlaySE(SoundManager.Sound.SpawnPaper);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Ground" && !isLanding)
        {
            //地面と接触した
            isLanding = true;
        }
    }

}
