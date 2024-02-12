using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    public Item item;
    public GameObject visual;

    public void InteractWithObject()
    {
        visual.SetActive(false);
        Invoke(nameof(Destroy), 10);
    }

    [ServerRpc(RequireOwnership = false)]
    public void InteractWithObjectServerRPC()
    {
        visual.SetActive(false);
        Invoke(nameof(Destroy), 10);
    }


}
