using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Windows.Speech;
using Unity.VisualScripting;

public class GunNetwork : NetworkBehaviour
{
    public Camera cam;
    public float maxDist;
    public int damage;
    public GameObject hitmark;
    public float fireRate;
    public float nextTimeToShoot = 0f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            cam.transform.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && IsOwner && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDist))
            {
                shootServerRPC(cam.transform.forward);
            }      
        }
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void shootServerRPC(Vector3 rotation)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, rotation, out hit, maxDist))
        {
            if (hit.transform.GetComponentInParent<PlayerNetwork>())
            {
                hit.transform.GetComponentInParent<PlayerNetwork>().HealthPoint.Value -= damage;
                //Debug.Log(hit.transform.GetComponentInParent<PlayerNetwork>().HealthPoint.Value);
            }

            //Debug.Log(hit.transform.name);
            GameObject instBullet = Instantiate(hitmark, hit.point, hit.transform.rotation);
            instBullet.GetComponent<NetworkObject>().Spawn();
        }
    }
}
