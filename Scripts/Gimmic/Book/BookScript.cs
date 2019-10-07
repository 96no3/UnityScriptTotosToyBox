using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject book;
    public Transform f_book;
    [SerializeField]
    private float time = 1.0f;         //変位時間

    [HideInInspector] public bool isActive = false;
    private bool isFirst = true;
    private Quaternion initRot;
    private float diffTime = 0.0f;
    private float rate;

    private void Awake()
    {
        isActive = false;
        isFirst = true;
        diffTime = 0.0f;
    }

    void Start()
    {        
        initRot = book.transform.localRotation;
    }

    //f_bookの角度までbookを回転させる
    void FixedUpdate()
    {
        if (isActive)
        {
            diffTime += Time.fixedDeltaTime;

            if (diffTime > time)
            {
                book.transform.localRotation = f_book.transform.localRotation;

                diffTime = 0.0f;
                isActive = false;
            }

            rate = diffTime / time;

            if (rate > 0)
            {
                book.transform.localRotation = Quaternion.Slerp(initRot, f_book.transform.localRotation, rate);
            }
        }
    }

    public void Intrigger()
    {
        if (isFirst)
        {
            isActive = true;
            isFirst = false;
        }
    }
}
