using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Interact : NetworkBehaviour
{
    public Camera cam;
    public float maxDist;
    private Item item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithGen();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            InteractWithObject();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            InteractWithOil();
        }
    }

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
                
                bool canUse = InventoryManager.instance.CheckForItem(item);
                Debug.Log(canUse);
                if (canUse && item == genBehaviour.fuelItem && genBehaviour.hasFuel.Value == false)
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
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {
            OilSpillBehaviour oil = hit.transform.GetComponent<OilSpillBehaviour>();
            bool canUse = InventoryManager.instance.CheckForItem(item);
            Debug.Log(canUse);
            if (canUse && item == oil.item && oil.isOnFire == false)
            {
                Item receivedItem = InventoryManager.instance.GetSelectedItem(true);
                InteractWithOilServerRPC(cam.transform.forward);
                if (oil)
                {
                    oil.turnOnFire.Value = true;
                }
            }
            
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractWithOilServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, maxDist))
        {
            Debug.Log(hit);
            OilSpillBehaviour oil = hit.transform.GetComponent<OilSpillBehaviour>();

            if (oil)
            {
                oil.turnOnFire.Value = true;
            }
        }
    }

    #endregion

    #region General object interaction
    void InteractWithObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
        {

            item = hit.transform.GetComponent<ObjectInteract>().item;
            if (item)
            {
                bool canAdd = InventoryManager.instance.CheckForSpace(item);
                if (canAdd)
                {
                    item = hit.transform.GetComponent<ObjectInteract>().item;
                    hit.transform.GetComponent<ObjectInteract>().InteractWithObject();
                    InventoryManager.instance.AddItem(item);
                }
            }   
        }
    }
    #endregion
}
