using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects / LightController")]
public class LightControllerSO : ScriptableObject
{
    public Material materialOn;
    public Material materialOff;
    //public bool LightsOn;

    public float lightOnInt;
}
