using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerOptions", menuName = "ScriptableObjects/Player/Options", order = 1)]
public class PlayerOptions : ScriptableObject
{
    //public bool SSGI;
    public bool Bloom;
    public bool AO;
    public bool Fog;
    public bool SSR;
}
