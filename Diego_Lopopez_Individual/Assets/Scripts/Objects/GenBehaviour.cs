using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GenBehaviour : NetworkBehaviour
{
    public LightControllerSO lightControllerSO;
    public Item fuelItem;

    [Header("")]

    public GameObject smoke;
    public MeshRenderer lightMesh;
    public Light lightComp;
    public Material materialOff;
    public Material materialOn;

    public Animator animator;

    [Header("")]

    public NetworkVariable<bool> hasFuel = new NetworkVariable<bool>();

    [HideInInspector] public LightController_Level1 lightController;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        lightController = GameObject.Find("GameController").GetComponent<LightController_Level1>();
        if (!IsHost) return;
        hasFuel.Value = false;
    }

    private void Update()
    {
        if (!lightController) return;
        if (!lightControllerSO.LightsOn)
        {
            hasFuel.Value = false;
            lightMesh.material = materialOff;
            lightComp.color = Color.red;
            animator.Play(null);
            smoke.SetActive(false);
        }
        else
        {
            lightMesh.material = materialOn;
            lightComp.color = Color.green;
            animator.Play("GenOn");
            smoke.SetActive(true);
        }
    }

}


