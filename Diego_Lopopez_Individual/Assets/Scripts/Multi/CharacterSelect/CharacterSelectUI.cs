using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class CharacterSelectUI : NetworkBehaviour
{
    [Header("Opciones")]
    public Button readyButton;
    public Button mainMenuButton;
    public Button lobbySettingsButton;

    [Header("Niveles")]
    public Button level1Button;
    public Button mazeButton;
    public Button puzzleButton;

    [Header("Colores")]
    public Color color;
    public Color firstColor;

    private bool isReady;

    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        if (!IsHost)
        {
            level1Button.gameObject.SetActive(false);
            mazeButton.gameObject.SetActive(false);
            puzzleButton.gameObject.SetActive(false);

            lobbySettingsButton.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {


        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.LoadNetwork(Loader.Scene.LobbyScene);

        });

        firstColor = readyButton.GetComponent<Image>().color;

        readyButton.onClick.AddListener(() =>
        {
            if (!isReady)
            {
                CharacterSelectReady.Instance.SetPlayerReady();
                readyButton.GetComponent<Image>().color = color;
                isReady = true;
            }
            else
            {
                CharacterSelectReady.Instance.SetPlayerNotReady();
                readyButton.GetComponent<Image>().color = firstColor;
                isReady = false;
            }
            
        });

        level1Button.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.level1Map.Value = true;
            CharacterSelectReady.Instance.mazeMap.Value = false;
            CharacterSelectReady.Instance.puzzleMap.Value = false;
            level1Button.GetComponent<Image>().color = color;
            mazeButton.GetComponent<Image>().color = firstColor;
            puzzleButton.GetComponent<Image>().color = firstColor;
        });

        mazeButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.level1Map.Value = false;
            CharacterSelectReady.Instance.mazeMap.Value = true;
            CharacterSelectReady.Instance.puzzleMap.Value = false;
            level1Button.GetComponent<Image>().color = firstColor;
            mazeButton.GetComponent<Image>().color = color;
            puzzleButton.GetComponent<Image>().color = firstColor;
        });

        puzzleButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.level1Map.Value = false;
            CharacterSelectReady.Instance.mazeMap.Value = false;
            CharacterSelectReady.Instance.puzzleMap.Value = true;
            level1Button.GetComponent<Image>().color = firstColor;
            mazeButton.GetComponent<Image>().color = firstColor;
            puzzleButton.GetComponent<Image>().color = color;
        });
    }
}
