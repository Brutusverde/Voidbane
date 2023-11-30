using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class CharacterSelectUI : NetworkBehaviour
{

    public Button readyButton;
    public Button schoolButton;
    public Button mazeButton;
    public Color color;
    public Color firstColor;
    private bool isReady;

    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        if (!IsHost)
        {
            schoolButton.gameObject.SetActive(false);
            mazeButton.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
       


        firstColor = readyButton.GetComponent<Image>().color;

        readyButton.onClick.AddListener(() =>
        {
            if (!isReady)
            {
                CharacterSelectReady.Instance.SetPlayerReady();
                readyButton.GetComponent<Image>().color = color;
                isReady = true;
            }
            
        });

        schoolButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.schoolMap.Value = true;
            CharacterSelectReady.Instance.mazeMap.Value = false;
            schoolButton.GetComponent<Image>().color = color;
            mazeButton.GetComponent<Image>().color = firstColor;
        });

        mazeButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.schoolMap.Value = false;
            CharacterSelectReady.Instance.mazeMap.Value = true;
            schoolButton.GetComponent<Image>().color = firstColor;
            mazeButton.GetComponent<Image>().color = color;
        });
    }
}
