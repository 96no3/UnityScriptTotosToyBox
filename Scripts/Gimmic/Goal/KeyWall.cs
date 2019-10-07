using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyWall : MonoBehaviour
{
    public GameObject key;
    public GameObject uiKey;
    public GameObject smokeEffect;
    private float effectTime = 0;

    private AudioSource aud;
    [Header("SoundSE")]
    public AudioClip seHitPlayer;

    void Start()
    {
        effectTime = 0;
        aud = GetComponent<AudioSource>();
        smokeEffect.SetActive(false);
        key.transform.parent = null;
        smokeEffect.transform.parent = null;
        uiKey.transform.parent = null;
    }

    private void Update()
    {
        if (smokeEffect.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 1.5f)
            {
                smokeEffect.SetActive(false);
                effectTime = 0;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player"&& GameInstance.Instance.hasKey == true)
        {
            aud.PlayOneShot(seHitPlayer);
            smokeEffect.SetActive(true);
            key.SetActive(false);
            GameInstance.Instance.hasKey = false;
            gameObject.transform.localScale = Vector3.zero;
            uiKey.SetActive(false);
        }
    }
}
