using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaController : MonoBehaviour
{
    [SerializeField] private GameObject coinUi;

    //外部公開パラメータ
    public GameObject[] gachaBallPrefab;                ///< ガチャ玉のプレハブ
    [SerializeField] private int spawnMax = 5;          ///< ガチャの最大回数
    [SerializeField] private Transform spawnTransform;  ///< ガチャ玉がスポーンする地点
    [SerializeField] private float upForce = 0.4f;

    private List<GameObject> instancedGachaBall;        ///< インスタンス化済みのガチャ玉
    private bool isSpawn = false;
    private bool isInit = false;

    private AudioSource aud;
    [Header("SoundSE")]
    public AudioClip seHitPlayer;
    public AudioClip sePop;

    private void Start()
    {
        isSpawn = false;
        isInit = false;
        aud = GetComponent<AudioSource>();
        coinUi.SetActive(false);
        instancedGachaBall = new List<GameObject>();

        //ガチャカプセルのスポーン処理
        for (int i = 0; i < spawnMax; i++)
        {
            int index = Random.Range(0, gachaBallPrefab.Length);
            GameObject tmp = Instantiate(gachaBallPrefab[index], spawnTransform);
            tmp.transform.parent = null;
            tmp.SetActive(false);
            instancedGachaBall.Add(tmp);
        }
        isInit = true;
    }


    private void Update()
    {
        if (!isInit) return;

        if (isSpawn)
        {
            ActivateGachaBall();
        }

        if (instancedGachaBall.Count <= 0)
        {
            coinUi.SetActive(false);
        }
    }

    //ガチャ玉のアクティブ時の処理
    void ActivateGachaBall()
    {
        aud.PlayOneShot(sePop);
        List<GameObject> tmpList = new List<GameObject>();
        foreach (GameObject e in instancedGachaBall)
        {
            tmpList.Add(e);
            break;
        }

        foreach (GameObject e in tmpList)
        {
            //ガチャ玉のアクティブ化・物理演算の有効化
            e.SetActive(true);

            Rigidbody rb = e.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            //ガチャ玉を吹き飛ばす力の設定
            //Quaternion randRot = Quaternion.AngleAxis(/*Random.Range(-15, 15)*/1, Vector3.up);
            float angle = 80.0f;
            Quaternion randRot = Quaternion.AngleAxis(-angle, transform.right);

            Vector3 forcePower;
            forcePower = randRot * transform.forward;
            forcePower = transform.forward * 30.0f;

            rb.AddForce(forcePower);
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            rb.AddTorque(Vector3.right * 5.0f);

            isSpawn = false;
            instancedGachaBall.Remove(e);
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(instancedGachaBall.Count <= 0 || GameInstance.Instance.coinList.Count <= 0) return;

        if (other.tag == "Player" && isSpawn == false)
        {
            //プレイヤーと衝突した時の処理
            aud.PlayOneShot(seHitPlayer);
            List<Coin> tmpList = new List<Coin>();
            foreach (Coin e in GameInstance.Instance.coinList)
            {
                tmpList.Add(e);
                break;
            }
            foreach (Coin e in tmpList)
            {
                GameInstance.Instance.coinList.Remove(e);
                e.gameObject.SetActive(false);
                break;
            }

            isSpawn = true;
        }
    }
}
