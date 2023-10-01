using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LockerInteract : NetworkBehaviour
{
    public Camera cam;
    public MeshRenderer mRenderer;
    public NetworkVariable<bool> ColorOn = new NetworkVariable<bool>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            cam.transform.gameObject.SetActive(false);
        }
        mRenderer = GetComponentInChildren<MeshRenderer>();
        ColorOn.Value = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        
        if (ColorOn.Value == true)
        {
            mRenderer.enabled = true;
        }
        if (ColorOn.Value == false)
        {
            mRenderer.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20f))
            {
                lockerServerRPC(cam.transform.forward); 
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20f))
            {
                lockerServerRPC(cam.transform.forward);
            }
        }
    }

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
                    ColorOn.Value = false;

                }
                else if (locker.LockerFull.Value == true)
                {
                    locker.LockerFull.Value = false;
                    Debug.Log(hit.transform.GetComponent<LockerBehaviour>().LockerFull.Value);
                    ColorOn.Value = true;
                }

            }
        }
    }

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

    [ServerRpc(RequireOwnership = false)]
    private void goInServerRPC()
    {
        ColorOn.Value = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void goOutServerRPC()
    {
        ColorOn.Value = true;
    }

}
