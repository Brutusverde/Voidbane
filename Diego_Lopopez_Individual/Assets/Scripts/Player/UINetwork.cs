using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class UINetwork : NetworkBehaviour
{
    public Slider hpSlider;
    public Slider staminaSlider;
    public Slider sanitySlider;
    public PlayerNetwork playerNetwork;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI sanityText;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            hpText.gameObject.SetActive(false);
            staminaText.gameObject.SetActive(false);
            sanityText.gameObject.SetActive(false);

            hpSlider.gameObject.SetActive(false);
            staminaSlider.gameObject.SetActive(false);
            sanitySlider.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        hpSlider.value = playerNetwork.HealthPoint.Value;
        staminaSlider.value = playerNetwork.StaminaPoint.Value;
        sanitySlider.value = playerNetwork.SanityPoint.Value;

        hpText.text = Mathf.Round(playerNetwork.HealthPoint.Value).ToString();
        staminaText.text = Mathf.Round(playerNetwork.StaminaPoint.Value).ToString();
        sanityText.text = Mathf.Round(playerNetwork.SanityPoint.Value).ToString();
    }
}
