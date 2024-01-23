using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public LightControllerSO lightController;
    public Light _light;

    public float minTime;
    public float maxTime;

    public float minBigTime;
    public float maxBigTime;
    public float betTime;

    private float timer;
    private float bigTimer;
    private float thirdTimer;

    public MeshRenderer mesh1;
    public MeshRenderer mesh2;

    public GameObject sparks1;
    public GameObject sparks2;

    private Material[] matArray;

    private int flickInt;
    public bool thisLightFlickers;


    void Start()
    {
        sparks1.SetActive(false);
        sparks2.SetActive(false);

        flickInt = Random.Range(1, 2);

        if(flickInt == 1)
        {
            thisLightFlickers = true;
        }
        else
        {
            thisLightFlickers = false;
        }

        timer = Random.Range(minTime, maxTime);
        bigTimer = Random.Range(minTime, maxTime);
        matArray = mesh1.materials;
        thirdTimer = betTime;
    }


    void Update()
    {
        if (!thisLightFlickers) return;
        if(!lightController.LightsOn)return;

        TimeBetweenFlickerLight();
        FlickerLight();
    }

    void TimeBetweenFlickerLight()
    {
        if (bigTimer > 0)
        {
            bigTimer -= Time.deltaTime;
        }
        else if (bigTimer < 0)
        {
            thirdTimer -= Time.deltaTime;
            if(thirdTimer <= 0)
            {
                bigTimer = Random.Range(minBigTime, maxBigTime);
                matArray[0] = lightController.materialOn;
                _light.enabled = true;
                mesh1.materials = matArray;
                mesh2.materials = matArray;

                thirdTimer = betTime;
            }
            
        }
    }

    void FlickerLight()
    {
        if (bigTimer > 0) return;
        if(timer > 0)
        {
            timer -= 1 * Time.deltaTime;
        }

        if(timer <= 0)
        {
            _light.enabled = !_light.enabled;
            if(_light.enabled == true)
            {
                matArray[0] = lightController.materialOn;
                mesh1.materials = matArray;
                mesh2.materials = matArray;
                sparks1.SetActive(true);
                sparks2.SetActive(true);
            }
            else
            {
                matArray[0] = lightController.materialOff;
                mesh1.materials = matArray;
                mesh2.materials = matArray;
                sparks1.SetActive(false);
                sparks2.SetActive(false);
            }
           
            timer = Random.Range(minTime, maxTime);
        }
    }
}
