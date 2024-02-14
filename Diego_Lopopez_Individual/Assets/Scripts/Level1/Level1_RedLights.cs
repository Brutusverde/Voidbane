using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_RedLights : MonoBehaviour
{
    public LightControllerSO lightController;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightController.LighIsRed = true;
            lightController.LightsOn = false;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
