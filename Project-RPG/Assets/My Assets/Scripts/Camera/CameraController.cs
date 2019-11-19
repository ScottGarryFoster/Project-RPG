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
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Floor")
            inputCO.SimulateMouse(0, -0.1f);
                //transform.Rotate(Vector3.up, -1 * Time.deltaTime);
                //cameraOrbit.transform.rotation = Quaternion.Euler(0, 0, cameraOrbit.transform.rotation.z + 1f);

            //cameraOrbit.transform.position = new Vector3(cameraOrbit.transform.position.x, cameraOrbit.transform.position.y + 0.1f, cameraOrbit.transform.position.z);
    }
}
