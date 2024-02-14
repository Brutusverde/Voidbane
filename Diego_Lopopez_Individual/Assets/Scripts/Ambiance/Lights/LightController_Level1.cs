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

    private NetworkVariable<bool> countDownStarted = new NetworkVariable<bool>();

    [Space]

    public Texture2D[] darkLightmapDir, darkLightmapColor;
    public Texture2D[] brightLightmapDir, brightLightmapColor;

    private LightmapData[] darkLightmap, brightLightmap;


    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsHost) return;
        StartGenServerRPC();

        List<LightmapData> dlightmap = new List<LightmapData>();

        for (int i = 0; i < darkLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = darkLightmapDir[i];
            lmdata.lightmapColor = darkLightmapColor[i];
            dlightmap.Add(lmdata);
        }

        darkLightmap = dlightmap.ToArray();


        List<LightmapData> blightmap = new List<LightmapData>();

        for (int i = 0; i < brightLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();
            lmdata.lightmapDir = brightLightmapDir[i];
            lmdata.lightmapColor = brightLightmapColor[i];
            blightmap.Add(lmdata);
        }

        brightLightmap = blightmap.ToArray();
        LightmapSettings.lightmaps = brightLightmap;
    }

    //Turn on gens
    [ServerRpc(RequireOwnership = false)]
    public void StartGenServerRPC()
    {
        lightController.LightsOn = true;
    }

    //Turn off gens
    [ServerRpc(RequireOwnership = false)]
    public void StopGenServerRPC()
    {
        lightController.LightsOn = false;
    }



    void Update()
    {
        ChangeLightmap();
    }

    void ChangeLightmap()
    {
        if (lightController.LightsOn == false)
        {
            LightmapSettings.lightmaps = darkLightmap;
        }
        else
        {
            LightmapSettings.lightmaps = brightLightmap;
        }

        if (lightController.LighIsRed)
        {
            LightmapSettings.lightmaps = darkLightmap;
        }

        if (lightController.LighIsRed == false)
        {
            LightmapSettings.lightmaps = brightLightmap;
        }
    }

}
