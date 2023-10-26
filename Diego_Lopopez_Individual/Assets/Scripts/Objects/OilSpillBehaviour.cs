using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OilSpillBehaviour : NetworkBehaviour
{
    public GameObject fireObj;
    public bool isOnFire;
    public float timeToStop;
    public int damage;
    public bool givingDamage;
    public NetworkVariable<bool> turnOnFire = new NetworkVariable<bool>();


    // Start is called before the first frame update
    void Start()
    {
        fireObj.SetActive(false);
        isOnFire = false;
    }

    private void Update()
    {
        if(turnOnFire.Value == true && isOnFire == false)
        {
            fireOn();
        }
    }

    public void fireOn()
    {
        fireObj.SetActive(true);
        isOnFire = true;
        Invoke(nameof(fireOff), timeToStop);
    }

    public void fireOff()
    {
        fireObj.SetActive(false);
        isOnFire = false;
        turnOnFire.Value = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOnFire) return;

        Debug.Log("MmM");

        if (other.CompareTag("Player"))
        {
            Debug.Log("aaaaa");
            other.GetComponentInParent<PlayerNetwork>().HealthPoint.Value -= damage;
        }
    }
}
