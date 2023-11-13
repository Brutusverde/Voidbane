using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
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
