using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.Security.Cryptography;

public class Interact : NetworkBehaviour
{
    [Header("General Settings")]

    public Camera cam;
    public float maxDist;
    public float fireMaxDist;
    private Item item;

    [Header("Locker interaction")]

    //Cam components
    public PlayerCam playerCam;
    public Transform cameraHolder;
    public Headbob headbob;
    public DynamicDOF dof;
    public Volume dofVolume;

    //Player components
    public PlayerNetwork playerNetwork;
    public Rigidbody rb;
    public GameObject playerBody;

    //Capsule components
    public CapsuleCollider capsuleCollider;
    public Animator animator;

    [Header("Crosshair")]

    public GameObject crosshair;
    public Vector3 small;
    public Vector3 big;


    private void Update()
    {
        InteractWithCrosshair();

        if (Input.GetKeyDown(KeyCode.E))
        {
            GlobalInteract();
            InteractWithGen();
            InteractWithObject();
            InteractWithOil();
            InteractWithLocker();
            InteractWithDoor();
            InteractWithSlider();
        }
    }


    #region Crosshair interaction

    private void InteractWithCrosshair()
    {
        RaycastHit hit;
        RaycastHit hit2;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit2, fireMaxDist))
        {
            OilSpillBehaviour oil = hit2.transform.GetComponent<OilSpillBehaviour>();
            if (oil)
            {
                crosshair.transform.localScale = big;
            }
            else
            {
                crosshair.transform.localScale = small;
            }


            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
            {
                GenBehaviour gen = hit.transform.GetComponent<GenBehaviour>();
                ObjectBehaviour tryItem = hit.transform.GetComponent<ObjectBehaviour>();
                LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();
                DoorBehaviour Door = hit.transform.GetComponentInParent<DoorBehaviour>();


                if (gen || tryItem || locker || Door)
                {
                    crosshair.transform.localScale = big;
                }
                else
                {
                    crosshair.transform.localScale = small;
                }
            }
        }
        
        else
        {
            crosshair.transform.localScale = small;
        }
    }

    #endregion

    #region Gen interaction

    void InteractWithGen()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {
            GenBehaviour gen = hit.transform.GetComponent<GenBehaviour>();
            if (gen)
            {
                GenBehaviour genBehaviour = gen.GetComponent<GenBehaviour>();

                Item itemSelected = InventoryManager.instance.GetSelectedItem(false);

                bool canUse = InventoryManager.instance.CheckForItem(itemSelected);
                Debug.Log(canUse);
                if (canUse && itemSelected == genBehaviour.fuelItem)
                {
                    Debug.Log("HAsta aqui llegamos");
                    Item receivedItem = InventoryManager.instance.GetSelectedItem(true);
                    InteractWithGenServerRPC(cam.transform.forward);
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractWithGenServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, maxDist))
        {
            GenBehaviour gen = hit.transform.GetComponent<GenBehaviour>();
            if (gen)
            {
                gen.hasFuel.Value = true;
                gen.lightController.StartTimerServerRPC(); //Starts lights timer
                Debug.Log("Cargado");
            }
        }
    }

    #endregion

    #region Oil interaction

    void InteractWithOil()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, fireMaxDist))
        {
            OilSpillBehaviour oil = hit.transform.GetComponent<OilSpillBehaviour>();
            if (oil)
            {
                Item itemSelected = InventoryManager.instance.GetSelectedItem(false);

                bool canUse = InventoryManager.instance.CheckForItem(itemSelected);
                if (canUse && itemSelected == oil.item && oil.isOnFire == false)
                {
                    Item receivedItem = InventoryManager.instance.GetSelectedItem(true);
                    InteractWithOilServerRPC(cam.transform.forward);
                    oil.turnOnFire.Value = true;

                }
            }   
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractWithOilServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, fireMaxDist))
        {
            OilSpillBehaviour oil = hit.transform.GetComponent<OilSpillBehaviour>();
            oil.turnOnFire.Value = true;
        }
    }

    #endregion

    #region General object interaction
    void InteractWithObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {
            ObjectBehaviour tryItem = hit.transform.GetComponent<ObjectBehaviour>();
            if (tryItem)
            {
                item = hit.transform.GetComponent<ObjectBehaviour>().item;
                
                bool canAdd = InventoryManager.instance.CheckForSpace(item);
                if (canAdd)
                {
                    item = hit.transform.GetComponent<ObjectBehaviour>().item;
                    //if (!IsHost)
                    //{
                    //    hit.transform.GetComponent<ObjectInteract>().InteractWithObjectServerRPC();
                    //}
                    
                    hit.transform.GetComponent<ObjectBehaviour>().InteractWithObject();
                    
                    InventoryManager.instance.AddItem(item);
                } 
            }  
        }
    }
    #endregion

    #region Locker interaction
    public void InteractWithLocker()
    {
        if (!IsOwner) return;

        //Input for locker interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
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
                            headbob.enable = true;
                            playerNetwork.inLocker = false;
                            playerNetwork.speed = 2;
                            dofVolume.gameObject.SetActive(true);
                            dof.enabled = true;

                            //Leave player move the camera
                            playerCam.enabled = true;
                            //Return gravity to rb
                            rb.isKinematic = false;
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
                            headbob.enable = false;
                            playerNetwork.inLocker = true;
                            dofVolume.gameObject.SetActive(false);
                            dof.enabled = false;
                            //Turn off camera moving
                            playerCam.enabled = false;
                            //Quit gravity from rb
                            rb.isKinematic = true;
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
                            headbob.enable = true;
                            playerNetwork.inLocker = false;
                            playerNetwork.speed = 2;
                            dofVolume.gameObject.SetActive(true);
                            dof.enabled = true;
                            //Leave player move the camera
                            playerCam.enabled = true;
                            //Return gravity to rb
                            rb.isKinematic = false;
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
                            headbob.enable = false;
                            playerNetwork.inLocker = true;
                            dofVolume.gameObject.SetActive(false);
                            dof.enabled = false;
                            //Turn off camera moving
                            playerCam.enabled = false;
                            //Quit gravity from rb
                            rb.isKinematic = true;
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
        if (Physics.Raycast(cam.transform.position, rotation, out hit, maxDist))
        {
            if (hit.transform.GetComponent<LockerBehaviour>())
            {
                //Locker is full, put player outside
                LockerBehaviour locker = hit.transform.GetComponent<LockerBehaviour>();
                if (locker.LockerFull.Value == true)
                {
                    headbob.enable = true;
                    playerNetwork.inLocker = false;
                    playerNetwork.speed = 2;
                    dofVolume.gameObject.SetActive(true);
                    dof.enabled = true;
                    //Leave player move the camera
                    playerCam.enabled = true;
                    //Return gravity to rb
                    rb.isKinematic = false;
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
                    headbob.enable = false;
                    playerNetwork.inLocker = true;
                    dofVolume.gameObject.SetActive(false);
                    dof.enabled = false;
                    //Turn off camera moving
                    playerCam.enabled = false;
                    //Quit gravity from rb
                    rb.isKinematic = true;
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

    #endregion

    #region Door interaction

    private void InteractWithDoor()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {
            DoorBehaviour Door = hit.transform.GetComponentInParent<DoorBehaviour>();
            if (!Door) return;

            if (Door.doorOpen.Value == false)
            {
                Door.OpenDoorServerRPC();
            }

            else if (Door.doorOpen.Value == true)
            {
                Door.CloseDoorServerRPC();
            }
        }
    }

    #endregion

    #region Global interaction

    private void GlobalInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {
            if(hit.collider.TryGetComponent(out IInteractive Interactivo))
            {
                GlobalInteractServerRPC(cam.transform.forward);
                //Interactivo.Use();
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void GlobalInteractServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, maxDist))
        {
            if (hit.collider.TryGetComponent(out IInteractive Interactivo))
            {
                Interactivo.Use();
            }
        }
    }
    #endregion




    #region Slider interaction
    public void InteractWithSlider ()
    {
        if (!IsOwner) return;

        //Input for locker interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
            {
                if (hit.transform.GetComponent<SliderBehaviour>())
                {
                    
                    sliderServerRPC(cam.transform.forward);
                    SliderBehaviour slider = hit.transform.GetComponent<SliderBehaviour>();

                    //This is for visuals on clients side

                    //If you are the host
                    if (IsHost)
                    {
                        //Locker is full, put player outside
                        if (slider.SliderFull.Value == true)
                        {
                            playerBody.transform.position = new Vector3(slider.bodyPoint.position.x, playerBody.transform.position.y, slider.bodyPoint.position.z);
                        }

                        //Locker is empty, put player inside
                        if (slider.SliderFull.Value == false)
                        {
                            playerBody.transform.position = new Vector3(slider.bodyPoint.position.x, playerBody.transform.position.y, slider.bodyPoint.position.z);
                        }
                    }

                    //If you are the client
                    if (!IsHost)
                    {
                        //Locker is full, put player outside
                        if (slider.SliderFull.Value == false)
                        {
                            playerBody.transform.position = new Vector3(slider.bodyPoint.position.x, playerBody.transform.position.y, slider.bodyPoint.position.z);
                        }

                        //Locker is empty, put player inside
                        if (slider.SliderFull.Value == true)
                        {
                            playerBody.transform.position = new Vector3(slider.bodyPoint.position.x, playerBody.transform.position.y, slider.bodyPoint.position.z);
                        }
                    }
                }
            }
        }
    }

    //Server RPC for locker 
    [ServerRpc(RequireOwnership = false)]
    private void sliderServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, maxDist))
        {
            if (hit.transform.GetComponent<SliderBehaviour>())
            {
                //Locker is full, put player outside
                SliderBehaviour slider = hit.transform.GetComponent<SliderBehaviour>();
                if (slider.SliderFull.Value == false)
                {
                    playerBody.transform.position = new Vector3(slider.bodyPoint.position.x, playerBody.transform.position.y, slider.bodyPoint.position.z);
                }


                //Locker is empty, put player inside
                else if (slider.SliderFull.Value == true)
                {
                    playerBody.transform.position = new Vector3(slider.bodyPoint.position.x, playerBody.transform.position.y, slider.bodyPoint.position.z);
                }
            }
        }
    }

    #endregion
}
