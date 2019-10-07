using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    private Vector3 initScale;
    private float initDistance;


    void Start()
    {
        //初期スケールの取得
        initScale = gameObject.transform.localScale;
        initDistance = Vector3.Distance(startPos.position, endPos.position);
    }

    void FixedUpdate()
    {
        //初期スケールを基準に長さと角度の調整
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,
                                                      initScale.y * Vector3.Distance(startPos.position, endPos.position) / initDistance,
                                                      gameObject.transform.localScale.z);
        gameObject.transform.up = Vector3.Normalize(startPos.position - endPos.position);
    }
}
