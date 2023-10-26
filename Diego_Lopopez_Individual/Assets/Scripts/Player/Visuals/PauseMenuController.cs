using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PauseMenuController : NetworkBehaviour
{
    public GameObject pauseMenu;
    private bool menuIsOpen;
    public PlayerOptions options;
    public PlayerCam cam;

    [SerializeField]
    private Toggle SSGI_Toggle;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(menuIsOpen == false)
            {
                OpenMenu();
                checkState();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void OpenMenu()
    {
        pauseMenu.SetActive(true);
        menuIsOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cam.enabled = false;
    }

    private void CloseMenu()
    {
        pauseMenu.SetActive(false);
        menuIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.enabled = true;
    }

    ///////////////////////////////////////////Comportamiento de los botones

    public void togleSSGI()
    {
        if(options.SSGI == true)
        {
            options.SSGI = false;
        }
        else
        {
            options.SSGI = true;
        }
    }

    public void checkState()
    {
        if (options.SSGI == true)
        {
            SSGI_Toggle.isOn = true;
        }
        else
        {
            SSGI_Toggle.isOn = false;
        }
    }

}
