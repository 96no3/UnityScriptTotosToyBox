using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private SoundManager sound;
    private Rigidbody rb;
    private Player player;
    [SerializeField] private float upForce = 2.0f;

    private void Start()
    {
        sound = GameObject.FindGameObjectWithTag("SoundMgr").GetComponent<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sound.PlaySE(SoundManager.Sound.Trampoline);
            player.SetJump(true);
            rb.AddForce(Vector3.up * upForce, ForceMode.VelocityChange);
        }
    }
}
