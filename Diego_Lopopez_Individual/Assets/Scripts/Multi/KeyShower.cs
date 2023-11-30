using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyShower : MonoBehaviour
{

    public TestRelay TestRelay;
    public TextMeshProUGUI keyText;

    // Start is called before the first frame update
    void Start()
    {
        TestRelay = GameObject.Find("TestRelay").GetComponent<TestRelay>();
        keyText.text = "Key " + TestRelay.key;
    }
}
