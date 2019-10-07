using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSwingScript : MonoBehaviour
{
    [SerializeField] private GameObject jumpUi;

    [SerializeField]
    private float boost = 1.0f;
    [SerializeField]
    private float limit = 40.0f;

    [Header("Input")]
    public string verticalInput = "Player1_Vertical";
    public string jumpInput = "Player1_Jump";

    private Vector3 initPos;
    private Quaternion initRot;
    private Rigidbody rb;
    private Rigidbody rbPlayer;
    private bool onPlayer = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPos = transform.position;
        initRot = transform.rotation;
        jumpUi.SetActive(false);
    }
    
    void FixedUpdate()
    {
        if (onPlayer)
        {
            rbPlayer.transform.position = new Vector3(transform.position.x, transform.position.y + 0.025f, transform.position.z);
            //上下の入力で
            float v = Input.GetAxis(verticalInput);

            if (rbPlayer.transform.rotation.eulerAngles.y > 90 && rbPlayer.transform.rotation.eulerAngles.y <= 270)
            {
                v *= -boost;
            }
            else
            {
                v *= boost;
            }
     
            rb.AddForce(0, 0, v, ForceMode.Force);

            if (gameObject.transform.rotation.eulerAngles.x > limit && gameObject.transform.rotation.eulerAngles.x < 360 - limit)
            {
                rb.velocity = Vector3.zero;
                jumpUi.SetActive(true);
            }

            if (Input.GetButtonDown(jumpInput))
            {   // スペースキーを入力したら
                onPlayer = false;
                rbPlayer.velocity = Vector3.zero;
            }
        }
        else
        {
            jumpUi.SetActive(false);
            gameObject.transform.position = initPos;
            gameObject.transform.rotation = initRot;
            rb.isKinematic = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            rbPlayer = other.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            onPlayer = true;
        }
    }
}
