using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory_Window : MonoBehaviour
{
    [Header("Links")]
    [Tooltip("Link the input control which contains information on which type of scripts should run")]
    public InputControl InputControl;
    [Tooltip("The actual locations for Weapons/tools")]
    public GameObject[] InventoryToolsAndWeapons;
    [Tooltip("The actual locations for Item Slots")]
    public GameObject[] InventoryItemSlots;
    [Tooltip("The storage for the Inventory")]
    public Storage_Inventory StorageInventory;

    private bool HaveUpdated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!HaveUpdated) UpdateWindowFromInventory();

        if (Input.GetKeyUp("j"))
            InventoryItemSlots[5].GetComponent<UI_Inventory_Slot>().storageItem._name = "Window";

    }

    public void UpdateWindowFromInventory()
    {
        for(int i = 0; i < InventoryItemSlots.Length; i++)
            InventoryItemSlots[i].GetComponent<UI_Inventory_Slot>().SetItemSlot(StorageInventory.getInventoryItem(i));
        HaveUpdated = true;
    }
}
