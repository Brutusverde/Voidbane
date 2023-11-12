using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ObjectInteract : NetworkBehaviour
{
    public Item item;

    public void InteractWithObject()
    {
        
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void InteractWithObjectServerRPC()
    {
        Destroy(gameObject);
    }
}


