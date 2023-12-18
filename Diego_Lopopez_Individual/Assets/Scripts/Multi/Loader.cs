using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        Test2,
        Test3,
        Test3_1,
        LoadingScene,
        LobbyScene,
        CharacterSelectScene,
        PruebaPrimerNivel,
    }

    private static Scene targetScene;


    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
