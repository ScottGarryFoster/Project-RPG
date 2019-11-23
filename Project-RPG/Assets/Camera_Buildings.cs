using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Buildings : MonoBehaviour
{
    [Header("Links")]
    [Tooltip("Drag the play in")]
    public GameObject Player;
    [Tooltip("Drag the camera for this room")]
    public GameObject Cam;
    [Tooltip("Input control")]
    public InputControl _InputControl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null && Cam.active)
            Cam.transform.LookAt(Player.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Player.gameObject.tag)
        {
            Cam.GetComponent<Camera>().enabled = true;
            if(_InputControl != null)
                _InputControl.UpdateCamera(Cam);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == Player.gameObject.tag)
        {
            if (_InputControl != null)
                _InputControl.UpdateCamera(Cam);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Player.gameObject.tag)
        {
            Cam.GetComponent<Camera>().enabled = false;
            if (_InputControl != null)
                _InputControl.ResetCamera();
        }
    }
}
