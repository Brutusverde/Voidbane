using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LoadNextLevel : NetworkBehaviour

{
    public GameManager gameManager;
    public bool puzzleScene;
    public bool mazeScene;
    public GameObject loadingScreen;
    public float time;


    private void Start()
    {
        loadingScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextScene());
        }
    }


    public IEnumerator LoadNextScene()
    {
        if (puzzleScene)
        {
            //other.GetComponentInParent<NetworkObject>().Despawn();
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(time);
            gameManager.playerAlreadySpawned = true;
            Loader.LoadNetwork(Loader.Scene.Puzzle);
        }

        if (mazeScene)
        {
            loadingScreen.SetActive(true);
            yield return new WaitForSeconds(time);
            gameManager.playerAlreadySpawned = true;
            Loader.LoadNetwork(Loader.Scene.Laberinto);
        }
    }
}
