using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : NetworkBehaviour
{
    //[SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;


    private void Awake()
    {
        //serverBtn.onClick.AddListener(() =>
        //{
        //    NetworkManager.Singleton.StartServer();
        //});

        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            //hostBtn.gameObject.SetActive(false);
            //clientBtn.gameObject.SetActive(false);
            //cam.gameObject.SetActive(false);
            
        });

        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //hostBtn.gameObject.SetActive(false);
            //clientBtn.gameObject.SetActive(false);
            //cam.gameObject.SetActive(false);
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        });
    }

}
