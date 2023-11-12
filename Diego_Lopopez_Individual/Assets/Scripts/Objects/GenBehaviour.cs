using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GenBehaviour : NetworkBehaviour
{
    public NetworkVariable<bool> hasFuel = new NetworkVariable<bool>();
    public LightController lightController;
    public Item fuelItem;
    public LightControllerSO lightControllerSO;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        lightController = GameObject.Find("GameController").GetComponent<LightController>();
        if (!IsHost) return;
        hasFuel.Value = false;
        //lightController.StartTimerServerRPC();
    }

    private void Update()
    {
        if (!lightController) return;
        if (lightController.CountDown.Value <= 0)
        {
            hasFuel.Value = false;
        }
    }

}


