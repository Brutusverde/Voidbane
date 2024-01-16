using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PuzzleValve_Manager : NetworkBehaviour
{
    public PuzzleValve valve1;
    public PuzzleValve valve2;
    public PuzzleValve valve3;
    public PuzzleValve valve4;

    public GameObject water;
    public GameObject doorLock;
    public GameObject floorWater;
    public Animator doorAnim;

    public float waterSpeed;
    public float valveSpeed;

    private bool playAnim;



    private void Update()
    {
        water.transform.position += water.transform.up * waterSpeed * Time.deltaTime;

        //All valves wrong
        if (valve1.valveReady.Value == false && valve2.valveReady.Value == false && valve3.valveReady.Value == false && valve4.valveReady.Value == false)
        {
            waterSpeed = valveSpeed * 4;
        }

        //One valve right
        if (valve1.valveReady.Value == true && valve2.valveReady.Value == false && valve3.valveReady.Value == false && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == true && valve3.valveReady.Value == false && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == false && valve3.valveReady.Value == true && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == false && valve3.valveReady.Value == false && valve4.valveReady.Value == true)
        {
            waterSpeed = valveSpeed * 3;
        }

        //Two valves right
        if (valve1.valveReady.Value == true && valve2.valveReady.Value == true && valve3.valveReady.Value == false && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == true && valve2.valveReady.Value == false && valve3.valveReady.Value == true && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == true && valve2.valveReady.Value == false && valve3.valveReady.Value == false && valve4.valveReady.Value == true ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == true && valve3.valveReady.Value == true && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == false && valve3.valveReady.Value == true && valve4.valveReady.Value == true ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == true && valve3.valveReady.Value == false && valve4.valveReady.Value == true)

        {
            waterSpeed = valveSpeed * 2;
        }

        //Three valves right
        if (valve1.valveReady.Value == true && valve2.valveReady.Value == true && valve3.valveReady.Value == true && valve4.valveReady.Value == false ||
            valve1.valveReady.Value == true && valve2.valveReady.Value == false && valve3.valveReady.Value == true && valve4.valveReady.Value == true ||
            valve1.valveReady.Value == true && valve2.valveReady.Value == true && valve3.valveReady.Value == false && valve4.valveReady.Value == true ||
            valve1.valveReady.Value == false && valve2.valveReady.Value == true && valve3.valveReady.Value == true && valve4.valveReady.Value == true )

        {
            waterSpeed = valveSpeed * 1;
        }

        //All valves right
        if (valve1.valveReady.Value == true && valve2.valveReady.Value == true && valve3.valveReady.Value == true && valve4.valveReady.Value == true)
        {
            waterSpeed = valveSpeed * -50;
            doorLock.SetActive(false);
            floorWater.SetActive(false);

            doorAnim.SetBool("Open", true);
            //doorAnim.SetBool("Close", false);
        }
    }
}


