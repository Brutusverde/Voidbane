using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects / LightController")]
public class LightControllerSO : ScriptableObject
{
    public Material materialOn;
    public Material materialOff;
    public Material materialRed;
    public bool LightsOn;
    public bool LighIsRed;

    public float lightOnInt;
}
