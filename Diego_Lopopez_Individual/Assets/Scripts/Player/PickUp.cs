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


    // Update is called once per frame
    void Update()
    {
        //if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(heldObject == null)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    Debug.Log("El raycast se lanza");
                    PickupObjectServerRPC(transform.forward);
                }
            }
            else
            {
                DropObject();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowObject();
        }

        if (heldObject != null)
        {
            if (Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
            {
                Vector3 moveDirection = (holdArea.transform.position - heldObject.transform.position);

                MoveObjectServerRPC(moveDirection);
            }


        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.transform.position - heldObject.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveObjectServerRPC(Vector3 move)
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            heldObjRB.AddForce(move * pickupForce);
            Debug.Log("El Objeto se mueve");
            //Vector3 moveDirection = (holdArea.transform.position - heldObject.transform.position);
            //heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }




    [ServerRpc(RequireOwnership = false)]
    private void PickupObjectServerRPC(Vector3 rotation)
    {
        Debug.Log("El rpc se ejecuta");
        RaycastHit hit2;
        if (Physics.Raycast(transform.position, rotation, out hit2, pickupRange))
        {
            Debug.Log("El raycast 2 se lanza");
            GameObject pickObj = hit2.transform.gameObject;
            if (pickObj.GetComponent<Rigidbody>())
            {
                Debug.Log("Se supone que coge el objeto");
                heldObjRB = pickObj.GetComponent<Rigidbody>();
                heldObjRB.useGravity = false;
                heldObjRB.drag = 10;
                heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                heldObject = pickObj;
            }
        }
    }



    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObject = pickObj;
        }
    }

    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObject = null;
    }

    void ThrowObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.AddForce(throwForce * 100f * Time.deltaTime * transform.TransformDirection(Vector3.forward), ForceMode.Impulse);
        heldObject = null;
    }
}
