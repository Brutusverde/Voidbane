using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SliderBehaviour : NetworkBehaviour
{
    //public NetworkVariable<bool> SliderFull = new NetworkVariable<bool>();
    //public NetworkVariable<bool> SomeoneHere = new NetworkVariable<bool>();
    //public GameObject armor;
    //public Transform cameraPoint;
    public Transform bodyPoint;


    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //SliderFull.Value = false;
        //armor.SetActive(false);
    }

    //private void Update()
    //{
    //    if (SliderFull.Value == true)
    //    {
    //        armor.SetActive(true);
    //    }
    //    else
    //    {
    //        armor.SetActive(false);
    //    }
    //}



   
}
