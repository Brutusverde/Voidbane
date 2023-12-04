using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class CharacterSelectPlayer : MonoBehaviour
{
    public int playerIndex;
    public GameObject ReadyGameObject;
    public PlayerVisual playerVisual;
    public Button kickButton;


    private void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = TestRelay.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            TestRelay.Instance.KickPlayer(playerData.clientId);
        });
    }

    private void Start()
    {
        TestRelay.Instance.OnPlayerDataNetworkListChanged += TestRelay_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void TestRelay_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        bool isConnected;
        if (isConnected = TestRelay.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();
            PlayerData playerData = TestRelay.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            ReadyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            playerVisual.SetPlayerColor(TestRelay.Instance.GetPlayerColor(playerData.colorId));
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
