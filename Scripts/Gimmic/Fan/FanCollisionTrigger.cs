using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanCollisionTrigger : MonoBehaviour
{
    public float addforce = 4.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "syabon")
        {
            Bubble b = other.GetComponent<Bubble>();
            b.Fukitobasi(transform.forward * addforce);
        }
    }
}
