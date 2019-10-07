using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotoAnimEvent : MonoBehaviour
{
    [SerializeField] Player toto;

    void RunSE()
    {
        toto.PlayWalkSe();
    }

    void ClearFinish()
    {
        toto.ChangeShakeAnim();
    }
}
