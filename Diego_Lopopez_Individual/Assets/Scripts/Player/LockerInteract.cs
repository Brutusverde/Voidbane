using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LockerInteract : NetworkBehaviour
{
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.P))
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
                lockerDebugServerRPC(cam.transform.forward);
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
                hit.transform.GetComponent<LockerBehaviour>().LockerFull.Value = true;
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
}
