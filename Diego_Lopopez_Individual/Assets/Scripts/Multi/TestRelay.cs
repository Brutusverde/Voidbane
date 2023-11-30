using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
//using QFSW.QC;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.UI;
using System;

public class TestRelay : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    public string key;
    //public GameObject button1;
    //public GameObject button2;
    //public GameObject cam;
    //public GameObject console;
    public GameObject keyUI;
    //public GameObject inputCanvas;
    public TMP_InputField inputField;


    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    //[Command]
    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            key = joinCode;

            Debug.Log(joinCode);

            //button1.SetActive(false);
            //button2.SetActive(false);
            //cam.SetActive(false);
            //console.SetActive(true);
            if (keyUI)
            {
                keyUI.SetActive(true);

                keyText.text = ("Key: " + joinCode);
            }
           

            GUIUtility.systemCopyBuffer = joinCode;

            RelayServerData relayServerData = new RelayServerData(allocation, "udp");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        } 
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }


    public void StartMap()
    {
        Loader.LoadNetwork(Loader.Scene.Test2);
    }


    //[Command]
    public async void JoinRelay()
    {
        try
        {
            string joinCode = inputField.text;
            key = joinCode;
            if (inputField.text != null)
            {
                Debug.Log("Joining Relay With " + joinCode);
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                //button1.SetActive(false);
                //button2.SetActive(false);
                //cam.SetActive(false);
                //console.SetActive(true);

                if (keyUI)
                {
                    keyUI.SetActive(true);

                    keyText.text = ("Key: " + joinCode);
                }

                    

                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient();
                Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
            }
            else
            {
                return;
            }
            
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
