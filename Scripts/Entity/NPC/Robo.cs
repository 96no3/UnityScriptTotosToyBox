using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo : NPC
{
    public override void NPCInit()
    {
        targetPos = GameObject.FindWithTag("RoboTarget").transform.position;
        isActive = false;
    }

    public override void NPCUpdateHelp()
    {
        if (agent.enabled)
        {
            agent.enabled = false;
        }        
    }

    public override void NPCUpdateFollow()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }
        targetPos = GameObject.FindWithTag("RoboTarget").transform.position;
    }

    public override void NPCUpdateEscape()
    {
    }

}
