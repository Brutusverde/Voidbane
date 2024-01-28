using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Level1_Fire : NetworkBehaviour
{
    public GameObject fire;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            offFireServerRPC();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void offFireServerRPC()
    {
        NetworkObject.Destroy(fire);
        NetworkObject.Despawn(fire);
    }

}
