using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimEvent : MonoBehaviour
{
    public GameObject deadEffect;
    public GameObject deadOriginEffect;
    private float time = 0;
    private bool isAnim = false;
    [SerializeField] private float finishTime = 4.0f;

    private void Update()
    {
        if (isAnim)
        {
            time += Time.deltaTime;
        }

        if(time > finishTime)
        {
            isAnim = false;
            time = 0;
            DeadFinish();
        }
    }

    void DeadFinish()
    {
        transform.parent.gameObject.SetActive(false);
    }

    void JumpFinish()
    {        
        transform.parent.SendMessage("ChangeStateHelp");
    }

    void DeadEffectAnim()
    {
        isAnim = true;
        if (deadEffect)
        {
            deadEffect.SetActive(true);
        }
        if (deadOriginEffect)
        {
            deadOriginEffect.SetActive(true);
        }
    }    
}
