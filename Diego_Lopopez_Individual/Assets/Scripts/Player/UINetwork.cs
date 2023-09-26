using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class UINetwork : NetworkBehaviour
{
    public Slider slider;
    public PlayerNetwork playerNetwork;
    public TextMeshProUGUI text;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            text.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        slider.value = playerNetwork.HealthPoint.Value;
        text.text = playerNetwork.HealthPoint.Value.ToString();
    }
}
