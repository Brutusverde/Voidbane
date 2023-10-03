using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.Rendering;

public class PickUp : NetworkBehaviour
{
    public Transform holdArea;
    private GameObject heldObject;
    private Rigidbody heldObjRB;
    public float pickupRange;
    public float pickupForce;
    public float throwForce;
    public NetworkVariable<bool> iHaveIt = new NetworkVariable<bool>();


    // Update is called once per frame
    void Update()
    {
        //if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(iHaveIt.Value == false)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObjectServerRPC(transform.forward);
                }
            }

            else if (iHaveIt.Value == true)
            {
                DropObjectServerRPC();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 dir = transform.forward;
            ThrowObjectServerRPC(dir);
        }

        if (iHaveIt.Value == true)
        {
            Vector3 holdA = holdArea.transform.position;
            MoveObjectServerRPC(holdA);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void MoveObjectServerRPC(Vector3 holdA)
    {
        if (Vector3.Distance(heldObject.transform.position, holdA) > 0.1f)
        {
            Vector3 moveDirection = (holdA - heldObject.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PickupObjectServerRPC(Vector3 rotation)
    {
        RaycastHit hit2;
        if (Physics.Raycast(transform.position, rotation, out hit2, pickupRange))
        {
            GameObject pickObj = hit2.transform.gameObject;
            if (pickObj.GetComponent<Rigidbody>())
            {
                heldObjRB = pickObj.GetComponent<Rigidbody>();
                heldObjRB.useGravity = false;
                heldObjRB.drag = 10;
                heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                heldObject = pickObj;
                iHaveIt.Value = true;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropObjectServerRPC()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        iHaveIt.Value = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ThrowObjectServerRPC(Vector3 direction)
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.AddForce(throwForce * 1000f * Time.deltaTime * direction, ForceMode.Impulse);
        iHaveIt.Value = false;
    }
}
