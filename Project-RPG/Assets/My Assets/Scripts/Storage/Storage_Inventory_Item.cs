using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage_Inventory_Item : MonoBehaviour
{
    public Storage_Inventory storageInventory;
    public string _name = "";

    private int itemNumber = -1;
    private int quantity = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getItemNumber() { return itemNumber; }
    public int getQuantity() { return quantity; }
    public void addQuantity(int number)
    {
        quantity += number;
        UpdateQuantityRender();

    }
    public void setItem(int number)
    {
        itemNumber = number;
        UpdateQuantityRender();
    }

    public GameObject getItemPrefab()
    {
        GameObject prefab = storageInventory.getPrefabItems(itemNumber);
        if (prefab == null) return null;
        return prefab;
    }

    void UpdateQuantityRender()
    {
        
    }
}
