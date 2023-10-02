using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.Rendering;

public class LockerInteract : NetworkBehaviour
{
    
    public NetworkVariable<bool> InLocker = new NetworkVariable<bool>();

    //Cam components
    public Camera cam;
    public PlayerCam playerCam;
    public Transform cameraHolder;

    //Player components
    public PlayerNetwork playerNetwork;
    public GunNetwork gunNetwork;
    public Rigidbody rb;

    //Capsule components
    public CapsuleCollider capsuleCollider;
    public Animator animator;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        InLocker.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        //Input for locker interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20f))
            {
                if (hit.transform.GetComponent<LockerBehaviour>())
                {
                    lockerServerRPC(cam.transform.forward);

                    //This is for visuals on clients side

                    //If you are the host
                    if (IsHost)
                    {
                        //Locker is full, put player outside
                        if (InLocker.Value == false)
                        {
                            playerCam.enabled = true;
                            playerNetwork.enabled = true;
                            gunNetwork.enabled = true;
                            rb.useGravity = true;
                            capsuleCollider.isTrigger = false;
                            animator.SetBool("TurnOff", false);

                            cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
                        }

                        //Locker is empty, put player inside
                        if (InLocker.Value == true)
                        {
                            playerCam.enabled = false;
                            playerNetwork.enabled = false;
                            gunNetwork.enabled = false;
                            rb.useGravity = false;
                            capsuleCollider.isTrigger = true;
                            animator.SetBool("TurnOff", true);

                            LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();
                            cam.transform.SetPositionAndRotation(locker.cameraPoint.position, locker.cameraPoint.rotation);
                        }
                    }

                    //If you are the client
                    if (!IsHost)
                    {
                        //Locker is full, put player outside
                        if (InLocker.Value == true)
                        {
                            playerCam.enabled = true;
                            playerNetwork.enabled = true;
                            gunNetwork.enabled = true;
                            rb.useGravity = true;
                            capsuleCollider.isTrigger = false;
                            animator.SetBool("TurnOff", false);

                            cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
                        }

                        //Locker is empty, put player inside
                        if (InLocker.Value == false)
                        {
                            playerCam.enabled = false;
                            playerNetwork.enabled = false;
                            gunNetwork.enabled = false;
                            rb.useGravity = false;
                            capsuleCollider.isTrigger = true;
                            animator.SetBool("TurnOff", true);

                            LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();
                            cam.transform.SetPositionAndRotation(locker.cameraPoint.position, locker.cameraPoint.rotation);
                        }
                    }
                }   
            }
        }
    }

    //Server RPC for locker 
    [ServerRpc(RequireOwnership = false)]
    private void lockerServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, 20f))
        {
            if (hit.transform.GetComponent<LockerBehaviour>())
            {
                //Locker is empty, put player inside
                LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();
                if(locker.LockerFull.Value == false)
                {
                    locker.LockerFull.Value = true;
                    Debug.Log(hit.transform.GetComponent<LockerBehaviour>().LockerFull.Value);
                    
                    cam.transform.SetPositionAndRotation(locker.cameraPoint.position, locker.cameraPoint.rotation);

                    playerCam.enabled = true;
                    playerNetwork.enabled = true;
                    gunNetwork.enabled = true;
                    rb.useGravity = true;
                    capsuleCollider.isTrigger = false;

                    animator.SetBool("TurnOff", true);
                    InLocker.Value = true;
                }

                //Locker is full, put player outside
                else if (locker.LockerFull.Value == true)
                {
                    locker.LockerFull.Value = false;
                    Debug.Log(hit.transform.GetComponent<LockerBehaviour>().LockerFull.Value);
                    
                    cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);

                    playerCam.enabled = false;
                    playerNetwork.enabled = false;
                    gunNetwork.enabled = false;
                    rb.useGravity = false;
                    capsuleCollider.isTrigger = true;

                    animator.SetBool("TurnOff", false);
                    InLocker.Value = false;
                }
            }
        }
    }
}
