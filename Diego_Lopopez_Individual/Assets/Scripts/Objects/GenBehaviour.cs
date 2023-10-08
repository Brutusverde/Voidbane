using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GenBehaviour : NetworkBehaviour
{
    public NetworkVariable<bool> hasFuel = new NetworkVariable<bool>();
    public LightController lightController;

    private void Start()
    {
        lightController = GameObject.Find("GameController").GetComponent<LightController>();
        hasFuel.Value = true;
    }
}


