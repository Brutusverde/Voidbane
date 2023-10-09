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
    //public ReflectionProbe probe;
    public GameObject volume;
    public Color emissiveColor = new Color(255, 249, 239, 255);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        volume = GameObject.Find("FogDarkness");
        emissiveColor = new Color(255, 249, 239, 255);
    }

    private void Update()
    {
       
        controller = GameObject.Find("GameController").GetComponent<LightController>();
        if (controller.CountDown.Value <= 0)
        {
            _light.intensity = 0;
            lightBase.materials[2].SetColor("_EmissiveColor", emissiveColor * controller.emissiveIntensity.Value);
            if (!volume) return;
            volume.transform.gameObject.SetActive(true);
        }
        else
        {
            _light.intensity = controller.lightInt.Value;
            lightBase.materials[2].SetColor("_EmissiveColor", emissiveColor * controller.emissiveIntensity.Value);
            if (!volume) return;
            volume.transform.gameObject.SetActive(false);
        }
    }
}
