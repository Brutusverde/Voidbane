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

    private Coroutine co;


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
        if (other.CompareTag("Player"))
        {
            givingDamage = true;
            co = StartCoroutine(DamagePlayer(other.GetComponentInParent<PlayerNetwork>()));
            //other.GetComponentInParent<PlayerNetwork>().HealthPoint.Value -= damage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            givingDamage = false;
        }
    }


    private IEnumerator DamagePlayer(PlayerNetwork pn)
    {
        while (givingDamage)
        {
            yield return new WaitForSeconds(0.1f);
            pn.HealthPoint.Value -= damage;
        }

        StopCoroutine(co);
    }
}
