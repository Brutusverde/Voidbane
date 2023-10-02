using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.Rendering;

public class LockerInteract : NetworkBehaviour
{

    public float length;

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


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        //Input for locker interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, length))
            {
                if (hit.transform.GetComponent<LockerBehaviour>())
                {
                    lockerServerRPC(cam.transform.forward);
                    LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();

                    //This is for visuals on clients side

                    //If you are the host
                    if (IsHost)
                    {
                        //Locker is full, put player outside
                        if (locker.LockerFull.Value == false)
                        {
                            //Leave player move the camera
                            playerCam.enabled = true;
                            //Leave player move
                            playerNetwork.enabled = true;
                            //Leave player use weapon
                            gunNetwork.enabled = true;
                            //Return gravity to rb
                            rb.useGravity = true;
                            //Turn on physics
                            rb.Sleep();
                            //Return collider
                            capsuleCollider.isTrigger = false;
                            //Play animation
                            animator.SetBool("TurnOff", false);
                            //Move camera to player body
                            cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
                        }

                        //Locker is empty, put player inside
                        if (locker.LockerFull.Value == true)
                        {
                            //Turn off camera moving
                            playerCam.enabled = false;
                            //Turn off player moving
                            playerNetwork.enabled = false;
                            //Turn off player weapon
                            gunNetwork.enabled = false;
                            //Quit gravity from rb
                            rb.useGravity = false;
                            //Turn off physics
                            rb.Sleep();
                            //Turn off collider
                            capsuleCollider.isTrigger = true;
                            //Play animation
                            animator.SetBool("TurnOff", true);
                            //Move camera to player body
                            cam.transform.SetPositionAndRotation(locker.cameraPoint.position, locker.cameraPoint.rotation);
                        }
                    }

                    //If you are the client
                    if (!IsHost)
                    {
                        //Locker is full, put player outside
                        if (locker.LockerFull.Value == true)
                        {
                            //Leave player move the camera
                            playerCam.enabled = true;
                            //Leave player move
                            playerNetwork.enabled = true;
                            //Leave player use weapon
                            gunNetwork.enabled = true;
                            //Return gravity to rb
                            rb.useGravity = true;
                            //Turn on physics
                            rb.Sleep();
                            //Return collider
                            capsuleCollider.isTrigger = false;
                            //Play animation
                            animator.SetBool("TurnOff", false);
                            //Move camera to player body
                            cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
                        }

                        //Locker is empty, put player inside
                        if (locker.LockerFull.Value == false)
                        {
                            //Turn off camera moving
                            playerCam.enabled = false;
                            //Turn off player moving
                            playerNetwork.enabled = false;
                            //Turn off player weapon
                            gunNetwork.enabled = false;
                            //Quit gravity from rb
                            rb.useGravity = false;
                            //Turn off physics
                            rb.Sleep();
                            //Turn off collider
                            capsuleCollider.isTrigger = true;
                            //Play animation
                            animator.SetBool("TurnOff", true);
                            //Move camera to player body
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
        if (Physics.Raycast(cam.transform.position, rotation, out hit, length))
        {
            if (hit.transform.GetComponent<LockerBehaviour>())
            {
                //Locker is full, put player outside
                LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();
                if(locker.LockerFull.Value == true)
                {
                    //Leave player move the camera
                    playerCam.enabled = true;
                    //Leave player move
                    playerNetwork.enabled = true;
                    //Leave player use weapon
                    gunNetwork.enabled = true;
                    //Return gravity to rb
                    rb.useGravity = true;
                    //Turn on physics
                    rb.Sleep();
                    //Return collider
                    capsuleCollider.isTrigger = false;
                    //Play animation
                    animator.SetBool("TurnOff", false);
                    //Move camera to player body
                    cam.transform.SetPositionAndRotation(cameraHolder.position, cameraHolder.rotation);
                    locker.LockerFull.Value = false;
                }

                
                //Locker is empty, put player inside
                else if (locker.LockerFull.Value == false)
                {
                    //Turn off camera moving
                    playerCam.enabled = false;
                    //Turn off player moving
                    playerNetwork.enabled = false;
                    //Turn off player weapon
                    gunNetwork.enabled = false;
                    //Quit gravity from rb
                    rb.useGravity = false;
                    //Turn off physics
                    rb.Sleep();
                    //Turn off collider
                    capsuleCollider.isTrigger = true;
                    //Play animation
                    animator.SetBool("TurnOff", true);
                    //Move camera to player body
                    cam.transform.SetPositionAndRotation(locker.cameraPoint.position, locker.cameraPoint.rotation);
                    locker.LockerFull.Value = true;
                }
            }
        }
    }
}
