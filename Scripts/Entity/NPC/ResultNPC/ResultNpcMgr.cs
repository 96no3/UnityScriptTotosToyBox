using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultNpcMgr : MonoBehaviour
{
    [SerializeField] private ResultNPC[] npc;
    [SerializeField] private GameObject enemy;

    public void InitResultNpc()
    {
        enemy.SetActive(false);
        StartCoroutine("SetNpcAnim");
    }

    IEnumerator SetNpcAnim()
    {
        if (GameInstance.Instance.goalNpcList.Count == 0)
        {
            enemy.SetActive(true);
        }

        foreach (ResultNPC resultn in npc)
        {
            resultn.gameObject.SetActive(true);            

            if (resultn.id == 0 && GameInstance.Instance.goalNpcList.Count > 0)
            {
                resultn.SetIdle();
                resultn.isDead = false;
            }

            foreach (NPC n in GameInstance.Instance.goalNpcList)
            {
                if (resultn.id == n.id)
                {
                    resultn.SetIdle();
                    resultn.isDead = false;
                }
                yield return null;
            }

            if (resultn.isDead)
            {
                resultn.SetDead();
            }
            yield return null;
        }
        yield return null;
    }
}
