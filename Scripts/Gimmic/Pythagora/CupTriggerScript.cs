using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupTriggerScript : MonoBehaviour
{
    private SoundManager sound;

    [SerializeField] private EventGimmic eventGimmic;
    public GameObject ball;
    public GameObject ballMesh;
    public PulleyScript pulley;
    public GameObject target;
    public Transform targetCameraPos;

    private Rigidbody rb;
    private bool inCup = false;
    private bool isFirst = true;

    void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
        ballMesh = ball.transform.GetChild(0).gameObject;
        inCup = false;
        isFirst = true;
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    void FixedUpdate()
    {
        if (inCup)
        {
            if (pulley.GetValue())
            {
                ball.transform.position = target.transform.position;
                ballMesh.transform.position = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z);
                //ballMesh.SetActive(false);
                eventGimmic.eventCameraPos = targetCameraPos;
            }
            inCup = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isFirst)
        {
            if (other.tag == "Trigger")
            {
                rb.isKinematic = true;
                inCup = true;
                pulley.SetValue(true);
                isFirst = false;
                sound.PlaySE(SoundManager.Sound.Pythagora);
            }
        }        
    }
}
