using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public Color selectedColor;
    public Color notSelectedColor;


    public void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }


    public void Deselect()
    {
        image.color = notSelectedColor;
    }
}
