using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;    

public class DoorInteract : NetworkBehaviour
{
    public Transform cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if(Physics.Raycast(cam.position, cam.forward, out hit, 20f))
            {
                Debug.Log(hit.transform.name);
                DoorBehaviour Door = hit.transform.GetComponentInParent<DoorBehaviour>();
                if (!Door) return;

                if (Door.doorOpen.Value == false)
                {
                    Debug.Log("Door open");
                    Door.openDoor(cam.position);
                }

                else if (Door.doorOpen.Value == true)
                {
                    Debug.Log("Door close");
                    Door.closeDoor(cam.position);
                }
            }
        }
    }
}
