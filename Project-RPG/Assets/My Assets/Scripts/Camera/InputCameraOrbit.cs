using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCameraOrbit : MonoBehaviour
{
    public GameObject cameraOrbit;
    public GameObject player;
    public Camera cam;

    public float rotateSpeed = 8f;

    private Vector3 originalScale;
    private float scrollAmount = 0;
    private float seePlayerZoom = 0;
    private float lastPlayerSeeRefresh = 0;

    void Start()
    {
        originalScale = cameraOrbit.transform.localScale;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float h = rotateSpeed * Input.GetAxis("Mouse X");
            float v = rotateSpeed * Input.GetAxis("Mouse Y");

            SimulateMouse(h, v);
        }

        float scrollFactor = Input.GetAxis("Mouse ScrollWheel");
        if (scrollFactor != 0)
            scrollAmount += scrollFactor;

        cameraOrbit.transform.localScale = originalScale;
        //else
        //    seePlayerZoom = 0;

        if (scrollAmount != 0 || seePlayerZoom != 0)
            cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - (scrollAmount + seePlayerZoom));

        if (!CanSeePlayer())
        {
            if ((scrollAmount + seePlayerZoom) < 0.7f)
            {
                seePlayerZoom += 0.1f;
            }
        }
    }

    public void SimulateMouse(float h, float v)
    {
        if (cameraOrbit.transform.eulerAngles.z + v <= 0.1f || cameraOrbit.transform.eulerAngles.z + v >= 179.9f)
            v = 0;

        cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + h, cameraOrbit.transform.eulerAngles.z + v);
    }

    bool CanSeePlayer()
    {
        if (CastRay(0.5f, 0.5f)) return true;
        for(int i = 0; i < 5; i++)
        {
            Vector2 v2 = new Vector2(0.45f + (1 - i), 0.45f + (1 - i));
            if (CastRay(v2.x, v2.y)) return true;
        }
        for (int i = 0; i < 5; i++)
        {
            Vector2 v2 = new Vector2(0.5f + (1 - i), 0.45f + (1 - i));
            if (CastRay(v2.x, v2.y)) return true;
        }
        for (int i = 0; i < 5; i++)
        {
            Vector2 v2 = new Vector2(0.45f + (1 - i), 0.5f + (1 - i));
            if (CastRay(v2.x, v2.y)) return true;
        }
        for (int i = 0; i < 5; i++)
        {
            Vector2 v2 = new Vector2(0.5f + (1 - i), 0.5f + (1 - i));
            if (CastRay(v2.x, v2.y)) return true;
        }
        //Debug.Log("CAN NOT SEE PLAYER");
        return false;
    }

    bool CastRay(float x, float y, bool debug = false)
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(x, y, 0.5f));
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if (debug)
                Debug.Log("Hit: objectHit:" + objectHit.gameObject.name);

            if (hit.transform.gameObject.tag == "Player") return true;
        }
        return false;
    }
}
