using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoorBehaviour : NetworkBehaviour
{

    public NetworkVariable<bool> doorOpen = new NetworkVariable<bool>();
    public Animator animator;
    //public AudioClip audioClip;
    //public AudioClip audioClipClose;
    //public AudioSource audioSource;

    private void Start()
    {
        if (!IsHost) return;
        doorOpen.Value = false;
        animator.SetBool("Open", false);
        animator.SetBool("Close", false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OpenDoorServerRPC()
    {
        doorOpen.Value = true;
        animator.SetBool("Open", true);
        animator.SetBool("Close", false);

    }

    [ServerRpc(RequireOwnership = false)]
    public void CloseDoorServerRPC()
    {
        doorOpen.Value = false;
        animator.SetBool("Open", false);
        animator.SetBool("Close", true);
        //audioSource.clip = audioClipClose;
        //audioSource.Play();
    }
}
