using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public PlayerCam cam;
    public static InventoryManager instance;
    public PauseMenuController pauseMenuController;
    public Item[] startItems;

    private Item selectedItem;
    private bool hasSelectedItem;


    public InventorySlot[] inventorySlots;
    public InventorySlot inventorySlotShow;
    public GameObject inventoryItemPrefab;
    public int maxStack;

    int selectedSlot = -1;

    public GameObject inventory;
    public GameObject crossHair;

    public bool openInventory;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CloseInventory();
        ChangeSelectedSlot(0);
        foreach (var item in startItems)
        {
            AddItem(item);
        }
    }


    private void Update()
    {
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < inventorySlots.Length + 1)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (openInventory && pauseMenuController.menuIsOpen == false)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!openInventory && pauseMenuController.menuIsOpen == false)
            {
                CloseInventory();
            }
           
        }
    }

    void OpenInventory()
    {
        inventory.SetActive(true);
        crossHair.SetActive(false);
        openInventory = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cam.enabled = false;
    }

    void CloseInventory()
    {
        inventory.SetActive(false);
        crossHair.SetActive(true);
        openInventory = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.enabled = true;
    }


    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        InventoryItem inventoryItem = inventorySlots[newValue].GetComponentInChildren<InventoryItem>();
        if (inventoryItem)
        {
            selectedItem = inventorySlots[newValue].GetComponentInChildren<InventoryItem>().item;
            SelectedObject(selectedItem, inventorySlotShow);
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStack && itemInSlot.item.stackable == true)
            {

                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }



        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                if (hasSelectedItem == false)
                {
                    SelectedObject(item, inventorySlots[i]);
                    hasSelectedItem = true;
                }
                
                return true;
            }
        }
        return false;
    }


    public bool CheckForSpace(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStack && itemInSlot.item.stackable == true)
            {
                return true;
            }
        }



        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                return true;
            }
        }
        Debug.Log("No space for this item");
        return false;
        
    }


    public bool CheckForItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.count > 0)
            {
                //itemInSlot.count--;
                //itemInSlot.RefreshCount();
                return true;
            }
        }
        return false;
    }


    void SelectedObject(Item item, InventorySlot slot)
    {
        InventoryItem inventoryItem1 = inventorySlotShow.GetComponentInChildren<InventoryItem>();
        if (inventoryItem1)
        {
            Destroy(inventorySlotShow.GetComponentInChildren<InventoryItem>().gameObject);
        }
        GameObject newItemGo = Instantiate(inventoryItemPrefab, inventorySlotShow.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }


    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if(use == true)
            {
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                    Destroy(inventorySlotShow.GetComponentInChildren<InventoryItem>().gameObject);
                    hasSelectedItem = false;
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        else
        {
            return null;
        }
    }


}
