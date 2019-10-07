using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewScript : MonoBehaviour
{
    //操作するGameObject
    public GameObject ball;
    public GameObject obj;
    //移動先のTransform
    [SerializeField]
    private Transform f_obj;
    [SerializeField]
    private Transform ballPos;

    [SerializeField]
    private float time = 5.0f;         //変位時間

    public bool isActive = false;
    private float diffTime = 0.0f;
    private float rate;
    private Vector3 initPos;
    private Quaternion initRot;
    private Rigidbody rb;


    void Start()
    {
        isActive = false;
        diffTime = 0.0f;
        initPos = obj.transform.position;
        initRot = obj.transform.rotation;
        rb = ball.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            diffTime += Time.fixedDeltaTime;

            if (diffTime > time)
            {
                obj.transform.position = f_obj.position;
                obj.transform.rotation = f_obj.rotation;

                diffTime = 0.0f;
                rb.isKinematic = false;
                isActive = false;
            }

            rate = diffTime / time;

            if (rate > 0)
            {
                ball.transform.position = ballPos.position;
                obj.transform.position = Vector3.Lerp(initPos, f_obj.position, rate);
                obj.transform.rotation = Quaternion.Slerp(initRot, f_obj.rotation, rate);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Trigger")
        {
            rb.isKinematic = true;
            isActive = true;
        }
    }
}
