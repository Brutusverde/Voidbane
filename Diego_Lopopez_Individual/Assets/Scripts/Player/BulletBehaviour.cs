using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Windows.Speech;
using Unity.VisualScripting;

public class BulletBehaviour : NetworkBehaviour
{
    Rigidbody rb;
    public float speed;
    public int damage;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        rb = GetComponent<Rigidbody>();
        rb.velocity = this.transform.forward * speed;
    }

    public void OnTriggerEnter (Collider other)
    {
        if (!IsServer) return;
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerNetwork>().HealthPoint.Value -= damage;
            Debug.Log(other.GetComponentInParent<PlayerNetwork>().HealthPoint.Value);
        }
    }

   
}
