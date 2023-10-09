using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GenInteract : NetworkBehaviour
{
    public Camera cam;
    public float maxDist;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithGen();
        }
    }

    void InteractWithGen()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {
            InteractWithGenServerRPC(cam.transform.forward);
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
                gen.GetComponent<GenBehaviour>().lightController.StartTimerServerRPC();
            }
        }
    }

}
