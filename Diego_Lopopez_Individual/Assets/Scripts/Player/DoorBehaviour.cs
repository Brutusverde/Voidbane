using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoorBehaviour : NetworkBehaviour
{
    public Vector3 targetAngle = new Vector3(0f, 180f, 0f);
    private Vector3 currentAngle;
    public NetworkVariable<bool> doorOpen = new NetworkVariable<bool>();
    public Vector3 forward;
    public Vector3 startPosition;
    public Vector3 StartRotation;
    public float forwardDirection = 0;

    private void Start()
    {
        currentAngle = transform.eulerAngles;
        doorOpen.Value = false;
        forward = new Vector3(0, transform.position.y, 0);
        startPosition = transform.position;
        StartRotation = transform.rotation.eulerAngles;
    }

    public void openDoor( Vector3 UserPosition)
    {
        //transform.Rotate(new Vector3 (0, 1, 0), 90);
        doorOpen.Value = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;
        float dot = Vector3.Dot(forward, (UserPosition - transform.position).normalized);
        if(dot >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.z - 45, 0));
            transform.rotation = endRotation;
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.z + 45, 0));
            transform.rotation = endRotation;
        }
    }

    public void closeDoor(Vector3 UserPosition)
    {
        //transform.Rotate(new Vector3 (0, 1, 0), 90);
        doorOpen.Value = false;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);
        transform.rotation = endRotation;
    }
}
