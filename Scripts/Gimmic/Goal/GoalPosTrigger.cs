using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPosTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "NPC" || other.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.MovePosition(other.transform.position - Vector3.up * 0.2f);
            }
        }
    }
}
