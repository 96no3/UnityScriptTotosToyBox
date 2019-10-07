using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDominoScript : MonoBehaviour
{
    private SoundManager sound;

    [SerializeField] private GameObject arrow;

    public GameObject bookdomino;
    private List<BookScript> bookList;

    private bool isFirst = true;

    void Start()
    {
        isFirst = true;
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        bookList = new List<BookScript>();
        for (int i = 0; i < bookdomino.transform.childCount; i++)
        {
            bookList.Add(bookdomino.transform.GetChild(i).GetComponent<BookScript>());
        }
        arrow.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!isFirst) return;

            foreach(BookScript obj in bookList)
            {
                obj.Intrigger();
            }
            isFirst = false;
            arrow.SetActive(true);
            sound.PlaySE(SoundManager.Sound.FallBook);
        }        
    }
}
