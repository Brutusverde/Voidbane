using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PauseMenuController : NetworkBehaviour
{
    public GameObject pauseMenu;
    public PlayerOptions options;
    public PlayerCam cam;
    public GameObject crossHair;
    public InventoryManager inventoryManager;

    [Header("Buttons")]
    public Button SSGIButton;
    public Button BloomButton;
    public Button AOButton;
    public Button FogButton;
    public Button SSRButton;

    [Header("Colors")]
    public Color color1;
    public Color color2;

    public bool menuIsOpen;

    public override void OnNetworkSpawn()
    {
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(menuIsOpen == false && inventoryManager.openInventory)
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
        crossHair.SetActive(false);
    }

    private void CloseMenu()
    {
        pauseMenu.SetActive(false);
        menuIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.enabled = true;
        crossHair.SetActive(true);
    }

    ///////////////////////////////////////////Comportamiento de los botones

    public void close()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    #region Toggles
    public void togleSSGI()
    {
        if (options.SSGI == true)
        {
            options.SSGI = false;
            checkState();
        }
        else
        {
            options.SSGI = true;
            checkState();
        }
    }

    public void togleBloom()
    {
        if(options.Bloom == true)
        {
            options.Bloom = false;
            checkState();
        }
        else
        {
            options.Bloom = true;
            checkState();
        }
    }

    public void togleAO()
    {
        if (options.AO == true)
        {
            options.AO = false;
            checkState();
        }
        else
        {
            options.AO = true;
            checkState();
        }
    }

    public void togleFog()
    {
        if (options.Fog == true)
        {
            options.Fog = false;
            checkState();
        }
        else
        {
            options.Fog = true;
            checkState();
        }
    }

    public void togleSSR()
    {
        if (options.SSR == true)
        {
            options.SSR = false;
            checkState();
        }
        else
        {
            options.SSR = true;
            checkState();
        }
    }
    #endregion

    #region Check state
    public void checkState()
    {
        //State for SSGI
        if (options.SSGI == true)
        {
            SSGIButton.GetComponent<Image>().color = color1;
        }
        if (options.SSGI == false)
        {
            SSGIButton.GetComponent<Image>().color = color2;
        }

        //State for Bloom
        if (options.Bloom == true)
        {
            BloomButton.GetComponent<Image>().color = color1;
        }
        if (options.Bloom == false)
        {
            BloomButton.GetComponent<Image>().color = color2;
        }

        //State for AO
        if (options.AO == true)
        {
            AOButton.GetComponent<Image>().color = color1;
        }
        if (options.AO == false)
        {
            AOButton.GetComponent<Image>().color = color2;
        }

        //State for Fog
        if (options.Fog == true)
        {
            FogButton.GetComponent<Image>().color = color1;
        }
        if (options.Fog == false)
        {
            FogButton.GetComponent<Image>().color = color2;
        }

        //State for SSR
        if (options.SSR == true)
        {
            SSRButton.GetComponent<Image>().color = color1;
        }
        if (options.SSR == false)
        {
            SSRButton.GetComponent<Image>().color = color2;
        }
    }
    #endregion

}
