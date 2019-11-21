using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Items_Render : MonoBehaviour
{
    [Header("Links")]
    [Tooltip("The text for Quantity")]
    public GameObject QuantityText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuantity(int number)
    {
        if (QuantityText == null) return;
        QuantityText.GetComponent<TextMeshProUGUI>().text = number.ToString();
    }
}