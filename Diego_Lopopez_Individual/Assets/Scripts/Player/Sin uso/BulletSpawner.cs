using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    public GameObject bullet;
    public Transform spawner;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && IsOwner)
        {
            SpawnBulletServerRPC(spawner.position, spawner.rotation);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRPC(Vector3 position, Quaternion rotation) 
    {
        GameObject instBullet = Instantiate(bullet, position, rotation);
        instBullet.GetComponent<NetworkObject>().Spawn();
    }

}
