using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    public Item item;

    public void InteractWithObject()
    {
        Destroy(gameObject);
    }
}


