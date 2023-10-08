using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LightController : MonoBehaviour
{

    public GameObject[] lights;
    public bool lightOn;
    public Color emissiveColor;
    public ReflectionProbe probe;
    public Volume volume;
    public float emissiveIntensity;
    public float emissiveOn;
    public float emissiveOff;

    // Start is called before the first frame update
    void Start()
    {
        volume.enabled = true;
        lightOn = false;
        emissiveColor = new Color (255, 249, 239, 255);
        probe.enabled = false;
        probe.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(lightOn == true)
        {
            turnOnLights();
        }

        if (lightOn == false)
        {
            turnOffLights();
        }
    }

    void turnOffLights()
    {
        foreach (GameObject light in lights)
        {

            //float emissiveIntensity = 0;
            emissiveIntensity = emissiveOff;


            light.GetComponentInChildren<Light>().intensity = 0;
            light.GetComponentInChildren<MeshRenderer>().materials[2].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);

            probe.enabled = false;
            probe.enabled = true;
            volume.enabled = true;
        }
    }

    void turnOnLights()
    {
        foreach (GameObject light in lights)
        {
            //float emissiveIntensity = 300;
            emissiveIntensity = emissiveOn;

            probe.enabled = false;
            probe.enabled = true;
            volume.enabled = false;

            light.GetComponentInChildren<Light>().intensity = 100000;
            light.GetComponentInChildren<MeshRenderer>().materials[2].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);
        }
    }
}
