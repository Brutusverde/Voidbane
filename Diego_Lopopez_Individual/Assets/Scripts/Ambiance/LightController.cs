using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.Netcode;

public class LightController : NetworkBehaviour
{
    [Header("Light objects")]
    public GameObject[] lights;
    public float lightInt;
    public float emissiveOn;

    private Color emissiveColor = new Color(255, 249, 239, 255);
    private float emissiveIntensity;
    private float emissiveOff = 0;

    [Header("Ambiance")]
    public ReflectionProbe probe;
    public Volume volume;

    [Header("Timer")]
    public float countDownFull;
    public NetworkVariable<float> CountDown = new NetworkVariable<float>();

    private bool countDownStarted;

    [Header("Gen")]
    public GenBehaviour gen;

    // Start is called before the first frame update
    void Start()
    {
        //Lights start turned on
        CountDown.Value = countDownFull;
        emissiveColor = new Color(255, 249, 239, 255);
        probe.enabled = false;
        probe.enabled = true;
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        //This checks if the timer has started and takes 1 each second until it reaches 0
        if (countDownStarted == true)
        {
            gen.hasFuel.Value = false;
            CountDown.Value -= 1 * Time.deltaTime;

            //If the timer has finished, this turns off the lights
            if(CountDown.Value <= 0)
            {
                turnOffLights();
                countDownStarted = false;
                
            }
        }
    }

    //This starts the timer if the gen has fuel and it hadnt started yet. This is controled by the gen script
    public void StartTimer()
    {
        if (countDownStarted || gen.hasFuel.Value == false) return;
        turnOnLights();
        CountDown.Value = countDownFull;
        countDownStarted = true;
    }

    //This is the void that turns off the lights and changes the post processing
    void turnOffLights()
    {
        foreach (GameObject light in lights)
        {
            emissiveIntensity = emissiveOff;
            light.GetComponentInChildren<Light>().intensity = 0;
            light.GetComponentInChildren<MeshRenderer>().materials[2].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);
            probe.enabled = false;
            probe.enabled = true;
            volume.transform.gameObject.SetActive(true);
        }
    }

    //This is the void that turns on the lights and changes the post processing
    void turnOnLights()
    {
        foreach (GameObject light in lights)
        {
            emissiveIntensity = emissiveOn;

            probe.enabled = false;
            probe.enabled = true;
            volume.transform.gameObject.SetActive(false);

            light.GetComponentInChildren<Light>().intensity = lightInt;
            light.GetComponentInChildren<MeshRenderer>().materials[2].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);
        }
    }
}
