using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Unity.Netcode;
using TMPro;

public class LightController : NetworkBehaviour
{
    [Header("Ambiance")]
    public Volume volume;

    [Header("Timer")]
    public float countDownFull;
    public NetworkVariable<float> CountDown = new NetworkVariable<float>();

    public TextMeshProUGUI text;

    private NetworkVariable<bool> countDownStarted = new NetworkVariable<bool>();

    [Header("Gen")]
    public LightControllerSO lightController;


    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsHost) return;
        StartTimerServerRPC();
    }


    // Update is called once per frame
    void Update()
    {
        //This checks if the timer has started and takes 1 each second until it reaches 0
        if (countDownStarted.Value == true)
        {
            if (IsHost)
            {
                CountDown.Value -= 1 * Time.deltaTime;
                var roundCountDown = Mathf.Round(CountDown.Value);
                text.text = roundCountDown.ToString();
            }
            
            
            //If the timer has finished, this turns off the lights
            if(CountDown.Value <= 0)
            {
                if (!IsHost) return;
                countDownStarted.Value = false;
            }
        }
    }

    //This starts the timer if the gen has fuel and it hadnt started yet. This is controled by the gen script
    [ServerRpc(RequireOwnership = false)]
    public void StartTimerServerRPC()
    {
        CountDown.Value = countDownFull;
        countDownStarted.Value = true;
    }
}
