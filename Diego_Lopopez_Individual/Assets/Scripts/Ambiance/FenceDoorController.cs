using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceDoorController : MonoBehaviour, IInteractive
{
    public FenceDoorSO SO;
    public bool openDoor;
    public Animator animator;
    public AudioClip doorSound;
    public AudioSource audiosource;

    private void Start()
    {
        openDoor = false;
    }

    void Update()
    {
        if(SO.canOpen && SO.lightWasRed)
        {
            openDoor = true;
        }
    }

    public void Use()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (openDoor)
        {
            Debug.Log("Door is open");
            animator.Play("FenceDoorOpen");
            audiosource.clip = doorSound;
            audiosource.Play();
        }
    }

}
