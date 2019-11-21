using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory_Slot : MonoBehaviour
    , IPointerClickHandler // 2
{
    public Storage_Inventory_Item storageItem;

    private int itemNumber = -1;
    private int quantity = -1;
    private GameObject itemPrefab = null;
    private GameObject itemOwned = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(storageItem != null)
        {
            if (storageItem.getItemNumber() != itemNumber)
            {
                ClearSelf();
                SetupItemPrefab();
                itemNumber = storageItem.getItemNumber();
            }
            if(itemOwned != null)
                if (storageItem.getQuantity() != quantity)
                {
                    UI_Items_Render render = itemOwned.GetComponent<UI_Items_Render>();
                    if(render != null)
                    {
                        render.SetQuantity(storageItem.getQuantity());
                        quantity = storageItem.getQuantity();
                    }
                }
        }

    }

    void ClearSelf()
    {
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
    }

    void SetupItemPrefab()
    {
        if (storageItem == null) return;
        itemPrefab = storageItem.getItemPrefab();
        itemOwned = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        itemOwned.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void SetItemSlot(Storage_Inventory_Item item)
    {
        storageItem = item;
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        if (storageItem == null) return;

    }
}
