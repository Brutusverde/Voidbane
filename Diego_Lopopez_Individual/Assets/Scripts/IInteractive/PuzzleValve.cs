using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PuzzleValve : NetworkBehaviour, IInteractive
{
    private NetworkVariable<float> turnNumber = new NetworkVariable<float>();
    public NetworkVariable<bool> valveReady = new NetworkVariable<bool>();

    public GameObject valve;
    public float turnVal;
    //public float turnNumber;
    public float valveNumber;
    //public bool valveReady;

    private void Start()
    {
        turnNumber.Value = 1;
    }

    public void TurnValve()
    {
        valve.transform.RotateAround(valve.transform.position, new Vector3(1, 0, 0), turnVal * -1);
        turnNumber.Value++;
        if(turnNumber.Value >= 9)
        {
            turnNumber.Value = 1;
        }
        
        if(turnNumber.Value == valveNumber)
        {
            valveReady.Value = true;
        }
        else
        {
            valveReady.Value = false;
        }
    }

    public void Use()
    {
        TurnValve();
    }
}
