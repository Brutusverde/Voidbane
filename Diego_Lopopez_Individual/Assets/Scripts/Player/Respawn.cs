using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform spawnPoint;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact interact = other.GetComponentInParent<Interact>();
            interact.playerBody.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.transform.position.y, spawnPoint.position.z);
        }
    }
}
