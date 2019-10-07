using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookFallTrigger2 : MonoBehaviour
{
    private SoundManager sound;

    [SerializeField] private GameObject arrow;

    public BookScript book;
    private bool isFirst = true;

    private void Start()
    {
        isFirst = true;
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        arrow.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isFirst)
            {
                book.isActive = true;
                isFirst = false;
                sound.PlaySE(SoundManager.Sound.FallBook);
                arrow.SetActive(true);
            }
        }
    }
}
