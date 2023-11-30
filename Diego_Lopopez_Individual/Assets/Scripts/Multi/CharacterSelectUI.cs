using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{

    public Button readyButton;
    public Color color;
    private bool isReady;

    // Start is called before the first frame update
    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            if (!isReady)
            {
                CharacterSelectReady.Instance.SetPlayerReady();
                readyButton.GetComponent<Image>().color = color;
                isReady = true;
            }
            
        });
    }
}
