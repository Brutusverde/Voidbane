using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void school()
    {
        SceneManager.LoadScene("Test2");
    }

    public void maze()
    {
        SceneManager.LoadScene("Test3");
    }
}
