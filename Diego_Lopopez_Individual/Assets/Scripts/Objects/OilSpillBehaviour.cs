using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpillBehaviour : MonoBehaviour
{
    public GameObject fireObj;
    public bool isOnFire;

    // Start is called before the first frame update
    void Start()
    {
        fireObj.SetActive(false);
        isOnFire = false;
    }

    public void fireOn()
    {
        fireObj.SetActive(true);
        isOnFire = true;
    }

    public void fireOff()
    {
        fireObj.SetActive(false);
        isOnFire = false;
    }
}
