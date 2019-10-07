using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyScript : MonoBehaviour
{
    //プーリーでつながれているもの
    public GameObject obj1;
    public GameObject obj2;

    //移動先のTransform
    [SerializeField]
    private Transform f_obj1;
    [SerializeField]
    private Transform f_obj2;

    [SerializeField]
    private float time = 5.0f;         //変位時間

    [HideInInspector] public bool isActive = false;
    private float diffTime = 0.0f;
    private float rate;
    private Vector3 initobj1Pos;
    private Quaternion initobj1Rot;
    private Vector3 initobj2Pos;
    private Quaternion initobj2Rot;


    void Start()
    {
        isActive = false;
        diffTime = 0.0f;
        initobj1Pos = obj1.transform.position;
        initobj1Rot = obj1.transform.rotation;
        initobj2Pos = obj2.transform.position;
        initobj2Rot = obj2.transform.rotation;
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            diffTime += Time.fixedDeltaTime;

            if (diffTime > time)
            {
                obj1.transform.position = f_obj1.position;
                obj1.transform.rotation = f_obj1.rotation;
                obj2.transform.position = f_obj2.position;
                obj2.transform.rotation = f_obj2.rotation;

                diffTime = 0.0f;
                isActive = false;
            }

            rate = diffTime / time;

            if (rate > 0)
            {
                obj1.transform.position = Vector3.Lerp(initobj1Pos, f_obj1.position, rate);
                obj1.transform.rotation = Quaternion.Slerp(initobj1Rot, f_obj1.rotation, rate);
                obj2.transform.position = Vector3.Lerp(initobj2Pos, f_obj2.position, rate);
                obj2.transform.rotation = Quaternion.Slerp(initobj2Rot, f_obj2.rotation, rate);
            }
        }
    }

    public void SetValue(bool val)
    {
        isActive = val;
    }

    public bool GetValue()
    {
        return isActive;
    }
}
