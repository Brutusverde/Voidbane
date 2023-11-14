using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StairsBehaviour : MonoBehaviour
{
    public float speed;
    public bool onStairs;
    public bool canGoUp;
    public Transform playerPos;
    public PlayerNetwork player;

    private void Start()
    {
        onStairs = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canGoUp)
        {
            player = other.GetComponentInParent<PlayerNetwork>();
            if (player)
            {
                playerPos = player.transform.GetComponent<Transform>();
                onStairs = true;
                Debug.Log("Player");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && canGoUp)
        {
            PlayerNetwork playerNetwork = other.GetComponentInParent<PlayerNetwork>();
           
            if (player)
            {
                notOnStairs();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) && onStairs)
        {
            upStairs();
        }

        if (Input.GetKey(KeyCode.E) && !onStairs)
        {
            canGoUp = true;
        }

        if (Input.GetKey(KeyCode.LeftControl) && onStairs)
        {
            notOnStairs();
        }
    }

    void upStairs()
    {
        player.enabled = false;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * speed * 1000 * Time.deltaTime, ForceMode.Force);
        //playerPos.position += transform.up * speed * Time.deltaTime;
    }

    void notOnStairs()
    {
        player.enabled = true;
        player = null;
        playerPos = null;
        onStairs = false;
        canGoUp = false;

    }
        
}
