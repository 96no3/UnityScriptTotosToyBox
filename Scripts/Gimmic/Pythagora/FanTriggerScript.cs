using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTriggerScript : MonoBehaviour
{
    private SoundManager sound;

    [SerializeField] private Rigidbody rbBall;
    [SerializeField] private Animator fanAnim;

    [SerializeField]
    private float wait = 1.0f;         //待機時間

    [HideInInspector] public bool isActive = false;
    private bool isFirst = true;
    private float diffTime = 0.0f;

    private void Start()
    {
        diffTime = 0.0f;
        isActive = false;
        rbBall.isKinematic = true;
        isFirst = true;
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            diffTime += Time.fixedDeltaTime;

            if (diffTime > wait)
            {
                rbBall.isKinematic = false;
                isActive = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isFirst)
        {            
            if (other.tag == "Player")
            {
                fanAnim.SetBool("isActive", true);
                isActive = true;
                isFirst = false;
                sound.PlaySE(SoundManager.Sound.Pythagora);
            }
        }
        
    }
}
