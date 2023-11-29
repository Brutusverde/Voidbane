using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArmsRotation : NetworkBehaviour
{

    public GameObject arms;
    public Camera cam;


    // Update is called once per frame
    void Update()
    {
        arms.transform.rotation = cam.transform.rotation;
    }
}
