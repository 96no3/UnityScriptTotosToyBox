using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFallTrigger : MonoBehaviour
{
    public BookControll book;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            book.rb.isKinematic = false;
            book.col.isTrigger = true;
            gameObject.SetActive(false);
        }
    }
}
