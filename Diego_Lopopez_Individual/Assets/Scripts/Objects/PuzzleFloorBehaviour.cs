using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PuzzleFloorBehaviour : NetworkBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
