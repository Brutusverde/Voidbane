using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.Netcode;
using TMPro;

public class LightTurn : NetworkBehaviour
{
    public Light _light;
    public MeshRenderer  lightBase;
    public LightController controller;
    public Material materialOn;
    public Material materialOff;
    private GameObject volume;
    private Material[] matArray;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        volume = GameObject.Find("FogDarkness");
        matArray = lightBase.materials;
    }

    private void Update()
    {
       
        controller = GameObject.Find("GameController").GetComponent<LightController>();
        if (controller.CountDown.Value <= 0)
        {
            _light.intensity = 0;
            if (!volume) return;
            volume.transform.gameObject.SetActive(true);

            matArray[2] = materialOff;
            lightBase.materials = matArray;
        }
        else
        {
            _light.intensity = controller.lightInt.Value;
            if (!volume) return;
            volume.transform.gameObject.SetActive(false);
            matArray[2] = materialOn;
            lightBase.materials = matArray;
        }
    }
}
