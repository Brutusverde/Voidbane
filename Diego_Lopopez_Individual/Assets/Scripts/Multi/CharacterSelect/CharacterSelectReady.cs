using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }
    public NetworkVariable<bool> level1Map = new NetworkVariable<bool>();
    public NetworkVariable<bool> mazeMap = new NetworkVariable<bool>();
    public NetworkVariable<bool> puzzleMap = new NetworkVariable<bool>();

    private Dictionary<ulong, bool> playerReadyDictionary;

    public event EventHandler OnReadyChanged;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }


    public void SetPlayerReady()
    {
        SetPlayerReadyServerRPC();
    }

    public void SetPlayerNotReady()
    {
        SetPlayerNotReadyServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRPC(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRPC(serverRpcParams.Receive.SenderClientId);
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
            if(level1Map.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Level1);
            }
            else if(mazeMap.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.LevelRun);
            }
            else if (puzzleMap.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Puzzle);
            }

            else if (level1Map.Value == false && mazeMap.Value == false && puzzleMap.Value == false)
            {
                Loader.LoadNetwork(Loader.Scene.Level1);
            }

        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNotReadyServerRPC(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerNotReadyClientRPC(serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = false;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            if (level1Map.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Level1);
            }
            else if (mazeMap.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Laberinto);
            }
            else if (puzzleMap.Value == true)
            {
                Loader.LoadNetwork(Loader.Scene.Puzzle);
            }

            else if (level1Map.Value == false && mazeMap.Value == false && puzzleMap.Value == false)
            {
                Loader.LoadNetwork(Loader.Scene.Level1);
            }

        }
    }


    [ClientRpc]
    private void SetPlayerReadyClientRPC(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void SetPlayerNotReadyClientRPC(ulong clientId)
    {
        playerReadyDictionary[clientId] = false;
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }
}
