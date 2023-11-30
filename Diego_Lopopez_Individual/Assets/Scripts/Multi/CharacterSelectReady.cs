using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }
    public NetworkVariable<bool> schoolMap = new NetworkVariable<bool>();
    public NetworkVariable<bool> mazeMap = new NetworkVariable<bool>();

    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }


    public void SetPlayerReady()
    {
        SetPlayerReadyServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            if(schoolMap.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Test2);
            }
            else if(mazeMap.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Test3);
            }

            else if (schoolMap.Value == false && mazeMap.Value == false)
            {
                Loader.LoadNetwork(Loader.Scene.Test2);
            }

        }
    }
}
