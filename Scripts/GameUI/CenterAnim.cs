using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterAnim : MonoBehaviour
{
    [SerializeField] private StageMgr StageMgr;

    public void CenterFinishFinish()
    {
        StageMgr.GameEnd();
    }

    public void CenterStartFinish()
    {
        StageMgr.GameStart();
    }
}
