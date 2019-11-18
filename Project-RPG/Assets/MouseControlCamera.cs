using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlCamera : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public Transform player;

    private Vector3 offset;

    private float zoom = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(player.position.x, player.position.y + zoom, player.position.z + zoom);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            UpdateZoom();

        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }

    void UpdateZoom()
    {
        zoom += Input.mouseScrollDelta.y;
        if (zoom < 0)
            zoom = 0;
        else if (zoom > 5)
            zoom = 5;

        Debug.Log(zoom);

       offset = new Vector3(player.position.x, player.position.y + zoom, player.position.z + zoom);
    }
}
