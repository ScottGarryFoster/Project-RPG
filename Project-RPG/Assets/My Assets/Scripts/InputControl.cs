using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    public bool StopInputForUI = false;

    [Header("Links")]
    [Tooltip("Canvas Menu for the Pause screen")]
    public GameObject CanvasMenu;
    [Tooltip("Main Camera")]
    public GameObject MainCamera;

    [Header("Automatic Links")]
    [Tooltip("Current Camera will go here. Set automatically")]
    public GameObject CurrentCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (CurrentCamera == null && MainCamera != null)
            CurrentCamera = MainCamera;
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
            Time.timeScale = 1;
        }
        else
        {
            CanvasMenu.SetActive(true);
            StopInputForUI = true;
            Time.timeScale = 0;
        }
    }

    public void UpdateCamera(GameObject cam)
    {
        CurrentCamera = cam;
    }

    public void ResetCamera()
    {
        if (MainCamera != null)
            CurrentCamera = MainCamera;
    }
}
