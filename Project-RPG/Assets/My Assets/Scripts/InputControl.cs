using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public bool StopInputForUI = false;

    public GameObject CanvasMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            FlipUIMenu();
    }

    void FlipUIMenu()
    {
        if(StopInputForUI == true)
        {
            CanvasMenu.SetActive(false);
            StopInputForUI = false;
        }
        else
        {
            CanvasMenu.SetActive(true);
            StopInputForUI = true;
        }
    }
}
