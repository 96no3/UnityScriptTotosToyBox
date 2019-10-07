using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    
    void AttackHand()
    {
        enemy.SetCollider(true);
    }

    void HitEnd()
    {
        enemy.SetCollider(false);
    }
}
