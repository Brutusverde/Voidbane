using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.Netcode;
using TMPro;

public class LightController_Level1 : NetworkBehaviour
{
    public LightControllerSO lightController;

    [Header("Ambiance")]
    public UnityEngine.Rendering.Volume volume;

    [Header("Timer")]
    public NetworkVariable<bool> SwitchGen = new NetworkVariable<bool>();


    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsHost) return;
        StartGenServerRPC();
    }

    //Turn on gens
    [ServerRpc(RequireOwnership = false)]
    public void StartGenServerRPC()
    {
        SwitchGen.Value = true;
    }

    //Turn off gens
    [ServerRpc(RequireOwnership = false)]
    public void StopGenServerRPC()
    {
        SwitchGen.Value = false;
    }
}
