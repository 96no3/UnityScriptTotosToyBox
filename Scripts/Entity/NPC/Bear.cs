using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : NPC
{
    public override void NPCInit()
    {
        targetPos = GameObject.FindWithTag("BearTarget").transform.position;
        isActive = false;
    }

    public override void NPCUpdateHelp()
    {
    }

    public override void NPCUpdateFollow()
    {
        targetPos = GameObject.FindWithTag("BearTarget").transform.position;
    }

    public override void NPCUpdateEscape()
    {
    }

}
