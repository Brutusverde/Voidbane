using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LightTurnLevel1 : NetworkBehaviour
{
    public LightControllerSO lightController;
    [Header("")]

    public Light _light;

    public MeshRenderer light1;
    public MeshRenderer light2;

    private LightController lc;

    //private GameObject volume;
    //private Material[] matArray;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        lc = GameObject.Find("GameController").GetComponent<LightController>();
    }

    private void Update()
    {
        if (lc)
        {
            if (lc.CountDown.Value <= 0)
            {
                _light.intensity = 0;
                light1.material = lightController.materialOff;
                light2.material = lightController.materialOff;
                lightController.LightsOn = false;
            }
            else
            {
                _light.intensity = lightController.lightOnInt;
                light1.material = lightController.materialOn;
                light2.material = lightController.materialOn;
                lightController.LightsOn = true;
            }

            if (lightController.LighIsRed)
            {
                _light.intensity = lightController.lightOnInt;
                _light.color = Color.red;
                light1.material = lightController.materialRed;
                light2.material = lightController.materialRed;
            }

            if (lightController.LighIsRed == false)
            {
                _light.color = Color.white;
                _light.intensity = lightController.lightOnInt;
                light1.material = lightController.materialOn;
                light2.material = lightController.materialOn;
                lightController.LightsOn = true;
            }
        }



    }
}
