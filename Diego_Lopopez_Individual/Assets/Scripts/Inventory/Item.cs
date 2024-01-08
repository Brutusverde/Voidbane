using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenu(menuName = "ScriptableObjects / Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public bool stackable = true;
    public GameObject arms;
    public int itemNumber;
}
