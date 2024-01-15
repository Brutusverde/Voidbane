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
        LoadingScene,
        LobbyScene,
        CharacterSelectScene,
        Level1,
        Puzzle,
        Laberinto,
        LevelRun,
    }

    private static Scene targetScene;


    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }
}
