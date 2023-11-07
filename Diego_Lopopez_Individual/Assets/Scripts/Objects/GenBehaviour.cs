using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GenBehaviour : NetworkBehaviour
{
    public NetworkVariable<bool> hasFuel = new NetworkVariable<bool>();
    public LightController lightController;
    public Item fuelItem;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        lightController = GameObject.Find("GameController").GetComponent<LightController>();
        if (!IsHost) return;
        hasFuel.Value = true;
        lightController.StartTimerServerRPC();
    }

}


