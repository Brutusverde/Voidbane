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

        playerCam = GetComponentInChildren<PlayerCam>();
        cam = GetComponentInChildren<Camera>();
        playerNetwork = GetComponent<PlayerNetwork>();
        gunNetwork = GetComponent<GunNetwork>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();


        if (!IsOwner)
        {
            cam.transform.gameObject.SetActive(false);
            cameraHolder.gameObject.SetActive(false);
        }  
        InLocker.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        
        //Turn off mesh renderer
        if (InLocker.Value == true)
        {
            if (!IsLocalPlayer) return;

            //playerCam.enabled = false;
            playerNetwork.enabled = false;
            gunNetwork.enabled = false;
            rb.useGravity = false;
            capsuleCollider.isTrigger = true;

            animator.SetBool("TurnOff", true);
        }

        //Turn on mesh renderer
        if (InLocker.Value == false)
        {
            if (!IsLocalPlayer) return;

            //playerCam.enabled = true;
            playerNetwork.enabled = true;
            gunNetwork.enabled = true;
            rb.useGravity = true;
            capsuleCollider.isTrigger = false;

            animator.SetBool("TurnOff", false);
        }

        //Input for locker interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20f))
            {
                if (hit.transform.GetComponent<LockerBehaviour>())
                {
                    lockerServerRPC(cam.transform.forward); 
                }
                    
            }
        }

        //Input for locker debug
        if (Input.GetKeyDown(KeyCode.O))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20f))
            {
                lockerDebugServerRPC(cam.transform.forward);
            }
        }
    }

    //Server RPC for locker control
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
                    InLocker.Value = true;
                    playerCam.enabled = false;
                    //cam.transform.position = locker.cameraPoint.position;
                    //cam.transform.rotation = locker.cameraPoint.rotation;

                }

                //Locker is full, put player outside
                else if (locker.LockerFull.Value == true)
                {
                    locker.LockerFull.Value = false;
                    Debug.Log(hit.transform.GetComponent<LockerBehaviour>().LockerFull.Value);
                    InLocker.Value = false;
                    playerCam.enabled = true;
                    //cam.transform.position = cameraHolder.position;
                    //cam.transform.rotation = cameraHolder.rotation;
                }

            }
        }
    }

    //Server RPC for locker debug
    [ServerRpc(RequireOwnership = false)]
    private void lockerDebugServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, 20f))
        {
            if (hit.transform.GetComponent<LockerBehaviour>())
            {
                Debug.Log( hit.transform.GetComponent<LockerBehaviour>().LockerFull.Value);
            }
        }
    }
}
