using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class VolumeController : MonoBehaviour
{

    public Volume volume;
    public PlayerOptions playerOptions;

    private GlobalIllumination globalIlumination;
    private Bloom bloom;
    private ScreenSpaceAmbientOcclusion AO;
    private Fog fog;


    public bool SSGI_Active;
    public bool bloom_Active;
    public bool AO_Active;
    public bool fog_Active;


    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet<GlobalIllumination>(out globalIlumination);
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<ScreenSpaceAmbientOcclusion>(out AO);
        volume.profile.TryGet<Fog>(out fog);
    }

    // Update is called once per frame
    void Update()
    {
        //Global ilumination
        SSGI_Active = playerOptions.SSGI;

        if(SSGI_Active == true)
        {
            globalIlumination.active = true;
        }

        if (SSGI_Active == false)
        {
            globalIlumination.active = false;
        }

        //Bloom
        bloom_Active = playerOptions.Bloom;

        if (bloom_Active == true)
        {
            bloom.active = true;
        }

        if (bloom_Active == false)
        {
            bloom.active = false;
        }

        //AO
        AO_Active = playerOptions.AO;

        if (AO_Active == true)
        {
            AO.active = true;
        }

        if (AO_Active == false)
        {
            AO.active = false;
        }

        //Fog
        fog_Active = playerOptions.Fog;

        if (fog_Active == true)
        {
            fog.active = true;
        }

        if (fog_Active == false)
        {
            fog.active = false;
        }
    }
}
