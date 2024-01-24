using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WoodBehaviour : NetworkBehaviour, IInteractive
{
    public GameObject fireObj;
    public GameObject[] wood;
    public bool isOnFire;
    public float timeToStop;
    public NetworkVariable<bool> turnOnFire = new NetworkVariable<bool>();
    public Item item;

    private BoxCollider col;

    void Start()
    {
        fireObj.SetActive(false);
        isOnFire = false;
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (turnOnFire.Value == true && isOnFire == false)
        {
            fireOn();
        }
    }

    public void fireOn()
    {
        fireObj.SetActive(true);
        isOnFire = true;
        Invoke(nameof(fireOff), timeToStop);
    }

    public void fireOff()
    {
        for (int i = 0; i < wood.Length; i++)
        {
            wood[i].SetActive(false);
        }

        fireObj.SetActive(false);
        isOnFire = false;
        turnOnFire.Value = false;
        killServerRPC();
        Destroy(this.gameObject);
        
        //this.gameObject.SetActive(false);

        col.isTrigger = true;
    }

    public void Use()
    {
        Item itemSelected = InventoryManager.instance.GetSelectedItem(false);

        bool canUse = InventoryManager.instance.CheckForItem(itemSelected);
        if (canUse && itemSelected == item && isOnFire == false)
        {
            Item receivedItem = InventoryManager.instance.GetSelectedItem(true);
            fireOn();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void killServerRPC()
    {
        NetworkObject.Destroy(this.gameObject);
        NetworkObject.Despawn(this.gameObject);
    }
}


