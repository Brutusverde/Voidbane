using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LightTurn : NetworkBehaviour
{
    public LightControllerSO lightController;
    [Header("")]

    public Light _light;
    public MeshRenderer  lightBase;

    public MeshRenderer light1;
    public MeshRenderer light2;

    private LightController lc;

    private GameObject volume;
    private Material[] matArray;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        volume = GameObject.Find("FogDarkness");
        matArray = lightBase.materials;
        lc = GameObject.Find("GameController").GetComponent<LightController>();
    }

    private void Update()
    {
        if (lc)
        {
            if (lc.CountDown.Value <= 0)
            {
                _light.intensity = 0;
                if (!volume) return;
                volume.transform.gameObject.SetActive(true);
                matArray[2] = lightController.materialOff;
                lightBase.materials = matArray;
                lightController.LightsOn = false;
            }
            else
            {
                _light.intensity = lightController.lightOnInt;
                if (!volume) return;
                volume.transform.gameObject.SetActive(false);
                matArray[2] = lightController.materialOn;
                lightBase.materials = matArray;
                lightController.LightsOn = true;
            }
        }
        
    }
}
