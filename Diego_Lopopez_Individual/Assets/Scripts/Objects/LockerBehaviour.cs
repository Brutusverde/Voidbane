using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LockerBehaviour : NetworkBehaviour
{
    public NetworkVariable<bool> LockerFull = new NetworkVariable<bool>();
    public NetworkVariable<bool> SomeoneHere = new NetworkVariable<bool>();
    public GameObject armor;
    public Transform cameraPoint;


    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        LockerFull.Value = false;
        armor.SetActive(false);
    }

    private void Update()
    {
        if (LockerFull.Value == true)
        {
            armor.SetActive(true);
        }
        else
        {
            armor.SetActive(false);
        }
    }

    
}
