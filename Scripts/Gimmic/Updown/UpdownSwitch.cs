using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdownSwitch : MonoBehaviour
{
    private SoundManager sound;
    [SerializeField] private Updown up;

    private void Start()
    {
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (up.state == Updown.STATE.MOVE) return;
            up.ChangeMoveState();
            sound.PlaySE(SoundManager.Sound.Swich);
        }
    }
}
