using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearScript1 : MonoBehaviour
{
    [SerializeField] private ThirdPersonCamera hitCamera;

    public Material clearMaterial;
    private string hitName;
    private Material hitMaterial;
    private Dictionary<string,Material> hitObjectDictionary;

    private void Start()
    {
        hitObjectDictionary = new Dictionary<string, Material>();
    }

    void OnTriggerEnter(Collider other)
    {
        Renderer hitRenderer = other.gameObject.GetComponent<Renderer>();
        if (hitRenderer)
        {
            hitName = other.name;
            hitMaterial = other.gameObject.GetComponent<Renderer>().material;
            if(hitObjectDictionary.Count == 0 || !hitObjectDictionary.ContainsKey(hitName))
            {
                hitObjectDictionary.Add(hitName, hitMaterial);
            }
            other.gameObject.GetComponent<Renderer>().material = clearMaterial;            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hitCamera.isHit)
        {
            hitCamera.isHit = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (hitCamera.isHit)
        {
            hitCamera.isHit = false;
        }

        Renderer hitRenderer = other.gameObject.GetComponent<Renderer>();
        if (hitRenderer)
        {
            if (hitObjectDictionary.Count == 0) return;
            if (hitObjectDictionary.ContainsKey(other.name))
            {
                other.gameObject.GetComponent<Renderer>().material = hitObjectDictionary[other.name];
                //hitObjectDictionary.Remove(other.name);
            }
        }                    
    }

}

