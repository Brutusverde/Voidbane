using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OilSpillInteract : NetworkBehaviour
{
    public Transform cam;
    public float maxDist;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        InteractWithGen();
    //    }
    //}


    //void InteractWithGen()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(cam.position, cam.forward, out hit, 20f))
    //    {
    //        InteractWithGenServerRPC(cam.forward);
    //    }
    //}

    //[ServerRpc(RequireOwnership = false)]
    //private void InteractWithGenServerRPC(Vector3 rotation)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(cam.position, rotation, out hit, maxDist))
    //    {
    //        Debug.Log("Tumama");
    //        OilSpillBehaviour oilSpill = hit.transform.GetComponent<OilSpillBehaviour>();
    //        oilSpill.fireOn();
    //    }
    //}
}
