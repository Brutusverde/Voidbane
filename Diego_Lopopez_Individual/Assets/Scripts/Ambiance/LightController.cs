using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.Netcode;
using TMPro;

public class LightController : NetworkBehaviour
{
    [Header("Light objects")]
    public GameObject[] lights;
    //public float lightInt;
    //public float emissiveOn;

    public Color emissiveColor = new Color(255, 249, 239, 255);
    //private float emissiveIntensity;
    //private float emissiveOff = 0;

    [Header("Ambiance")]
    public ReflectionProbe probe;
    public Volume volume;

    [Header("Timer")]
    public float countDownFull;
    public NetworkVariable<float> CountDown = new NetworkVariable<float>();
    public TextMeshProUGUI text;

    public NetworkVariable<bool> countDownStarted = new NetworkVariable<bool>();
    //public bool countDownStarted;

    [Header("Gen")]
    public GenBehaviour gen;




    public NetworkVariable<float> lightInt = new NetworkVariable<float>();
    public NetworkVariable<float> emissiveOn = new NetworkVariable<float>();
    public NetworkVariable<float> emissiveIntensity = new NetworkVariable<float>();
    public NetworkVariable<float> emissiveOff = new NetworkVariable<float>();





    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        emissiveColor = new Color(255, 249, 239, 255);
        probe.enabled = false;
        probe.enabled = true;
        if (!IsHost) return;
        CountDown.Value = countDownFull;
        
    }


    // Update is called once per frame
    void Update()
    {
        //This checks if the timer has started and takes 1 each second until it reaches 0
        if (countDownStarted.Value == true)
        {
            if (IsHost)
            {
                gen.hasFuel.Value = false;
                CountDown.Value -= 1 * Time.deltaTime;
                var roundCountDown = Mathf.Round(CountDown.Value);
                text.text = roundCountDown.ToString();
            }
            
            
            //If the timer has finished, this turns off the lights
            if(CountDown.Value <= 0)
            {
                turnOffLightsServerRPC();
                if (!IsHost) return;
                countDownStarted.Value = false;
            }
        }
    }

    //This starts the timer if the gen has fuel and it hadnt started yet. This is controled by the gen script
    [ServerRpc(RequireOwnership = false)]
    public void StartTimerServerRPC()
    {
        if (/*countDownStarted.Value == true ||*/ gen.hasFuel.Value == false) return;
        turnOnLightsServerRPC();
        CountDown.Value = countDownFull;
        countDownStarted.Value = true;
    }


    //This is the void that turns off the lights and changes the post processing
    [ServerRpc(RequireOwnership = false)]
    void turnOffLightsServerRPC()
    { 
        //foreach (GameObject light in lights)
        //{
            emissiveIntensity = emissiveOff;
            //light.GetComponentInChildren<Light>().intensity = 0;
            //light.GetComponentInChildren<MeshRenderer>().materials[2].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity.Value);
            Debug.Log("Potato");
            probe.enabled = false;
            probe.enabled = true;
            //volume.transform.gameObject.SetActive(true); 
        //}
    }


    //This is the void that turns on the lights and changes the post processing
    [ServerRpc(RequireOwnership = false)]
    void turnOnLightsServerRPC()
    {
        //foreach (GameObject light in lights)
        //{
            emissiveIntensity = emissiveOn;

            probe.enabled = false;
            probe.enabled = true;
            //volume.transform.gameObject.SetActive(false);

            //light.GetComponentInChildren<Light>().intensity = lightInt.Value;
            //light.GetComponentInChildren<MeshRenderer>().materials[2].SetColor("_EmissiveColor", emissiveColor * emissiveIntensity.Value);
        //}
    }
}
