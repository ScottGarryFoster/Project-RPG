using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraOrbit;
    public Transform target;

    private InputCameraOrbit inputCO;

    void Start()
    {
        cameraOrbit.position = target.position;
        inputCO = cameraOrbit.GetComponent<InputCameraOrbit>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

        transform.LookAt(target.position);
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Floor")
            inputCO.SimulateMouse(0, -0.1f);//Simulates the mouse / camera moving up
    }
}
