using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public float spawnInterval = 2.0f;
    private float time = 0;
    public int spawnNum = 3;
    public GameObject spawnPrefab;
    private GameObject[] syabon;    
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Vector3 vec;

    void Start()
    {
        time = 0;
        startPos = transform.position;
        syabon = new GameObject[spawnNum];
        vec = new Vector3(transform.position.x, transform.position.y - 0.13f, transform.position.z);

        for(int i = 0; i < spawnNum; ++i)
        {
            syabon[i] = Instantiate(spawnPrefab, vec, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time >= spawnInterval)
        {
            foreach (GameObject gb in syabon)
            {
                if (gb.transform.position == vec)
                {
                    gb.transform.position = startPos;
                    gb.GetComponent<Bubble>().UpForce();
                    time = 0;
                    break;
                }
            }
        }
        
    }

}
