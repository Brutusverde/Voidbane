using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GenBehaviour : NetworkBehaviour
{
    public LightControllerSO lightControllerSO;
    public Item fuelItem;
    [Header("")]

    public NetworkVariable<bool> hasFuel = new NetworkVariable<bool>();

    [HideInInspector] public LightController lightController;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        lightController = GameObject.Find("GameController").GetComponent<LightController>();
        if (!IsHost) return;
        hasFuel.Value = false;
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


