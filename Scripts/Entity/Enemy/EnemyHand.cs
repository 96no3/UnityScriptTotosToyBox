using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    private float effectTime = 0;

    private void Start()
    {
        effectTime = 0;
    }

    private void Update()
    {
        if (hitEffectPrefab.activeInHierarchy)
        {
            effectTime += Time.deltaTime;
            if (effectTime > 0.5f)
            {
                hitEffectPrefab.SetActive(false);
                effectTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC" || other.tag == "Player")
        {
            hitEffectPrefab.SetActive(true);
        }
    }
}
