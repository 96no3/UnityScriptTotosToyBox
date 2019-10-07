using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotoResultAnimEvent : MonoBehaviour
{
    void ClearFinish()
    {
        GetComponent<Animator>().SetTrigger("Shake");
    }

    void RunSE()
    {

    }
}
