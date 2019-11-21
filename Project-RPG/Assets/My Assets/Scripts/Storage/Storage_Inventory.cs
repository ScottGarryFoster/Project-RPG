using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage_Inventory : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Link all the item prefabs")]
    public GameObject[] ItemPrefabs;

    private Storage_Inventory_Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 40; i++)
            gameObject.AddComponent<Storage_Inventory_Item>();
        items = gameObject.GetComponents<Storage_Inventory_Item>();
        for (int i = 0; i < items.Length; i++)
            items[i].storageInventory = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("h"))
            items[5]._name = "Inventory";
    }

    public Storage_Inventory_Item getInventoryItem(int index)
    {
        if (items.Length < index) return null;
        if (items[index] != null)
            return items[index];
        return null;
    }

    public GameObject getPrefabItems(int index)
    {
        if (ItemPrefabs.Length < index) return null;
        if (ItemPrefabs != null)
            return ItemPrefabs[index];
        return null;
    }

    public bool pickupItem(int number, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) continue;
            if (items[i].getItemNumber() == number)
            {
                items[i].addQuantity(quantity);
                return true;
            }
        }
        int freeSlot = findEarliestFreeSlot();
        if (freeSlot == -1) return false;
        items[freeSlot].setItem(number);
        items[freeSlot].addQuantity(quantity);
        return true;

    }

    int findEarliestFreeSlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) continue;
            if (items[i].getItemNumber() == -1)
                return i;
        }
        return -1;
    }
}
