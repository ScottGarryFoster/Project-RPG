using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes_Inventory : MonoBehaviour
{
    public Storage_Inventory StorageInventory;

    private int QuanOakWood; //How much Oak wood do you have
    private bool SeenOakWood = false;//Detects if we've ever seen wood in this scene.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool giveItem(int item, int number)
    {
        switch(item)
        {
            case 1: return giveOakWood(number);
        }
        return false;
    }

    public bool giveOakWood(int number)
    {
        SeenOakWood = true;
        if (StorageInventory != null) StorageInventory.pickupItem(0, number);
        return false;
    }
}
