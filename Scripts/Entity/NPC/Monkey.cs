using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : NPC
{
    [Header("movePoint")]
    [SerializeField] private GameObject pointObject;

    public override void NPCInit()
    {
        targetPos = GameObject.FindWithTag("BananaTarget").transform.position;
        isActive = true;
        pointObject.transform.parent = null;
    }

    public override void NPCUpdateHelp()
    {
        anim.SetBool("IsMove", false);
        agent.speed = speed;
        agent.acceleration = acceleration;
        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択します
        if (!agent.pathPending && agent.remainingDistance < 0.05f)
        {
            GotoNextPoint();
        }
    }

    public override void NPCUpdateFollow()
    {
        targetPos = GameObject.FindWithTag("BananaTarget").transform.position;
    }

    public override void NPCUpdateEscape()
    {
    }

}
