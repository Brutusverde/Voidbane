using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    public int colorId;
    public Image image;
    public GameObject selectedGameObject;


    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            TestRelay.Instance.ChangePlayerColor(colorId);

        });
    }

    private void Start()
    {
        TestRelay.Instance.OnPlayerDataNetworkListChanged += Instance_OnPlayerDataNetworkListChanged;
        image.color = TestRelay.Instance.GetPlayerColor(colorId);
        UpdateIsSelected();
    }

    private void Instance_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if(TestRelay.Instance.GetPlayerData().colorId == colorId)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }
}
