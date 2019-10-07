using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFallStart : MonoBehaviour
{
    public BookScript book;

    private void Start()
    {
        book.isActive = true;
    }
}
