using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    public GameObject playerGoalPos;
    private StageMgr stageMgr;

    private void Start()
    {
        stageMgr = GameObject.FindGameObjectWithTag("StageMgr").GetComponent<StageMgr>();
        cameraObject.SetActive(false);
        playerGoalPos.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().position = playerGoalPos.transform.position;
            if (!stageMgr.isAddGoal)
            {
                stageMgr.isAddGoal = true;
                stageMgr.gameState = StageMgr.GameState.Clear;
                cameraObject.SetActive(true);

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (enemies.Length > 0)
                {
                    foreach (GameObject e in enemies)
                    {
                        e.GetComponent<Enemy>().enemyState = Enemy.EnemyState.None;
                    }
                }
            } 
        }
    }
}
