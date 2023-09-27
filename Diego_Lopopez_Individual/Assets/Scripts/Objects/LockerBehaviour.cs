using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LockerBehaviour : NetworkBehaviour
{
    public NetworkVariable<bool> LockerFull = new NetworkVariable<bool>();


    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        LockerFull.Value = false;
    }
}
