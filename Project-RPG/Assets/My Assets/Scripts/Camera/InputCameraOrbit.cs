using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCameraOrbit : MonoBehaviour
{
    public GameObject cameraOrbit;
    public GameObject player;
    public Camera cam;
    public GameObject player_head;

    public float rotateSpeed = 8f;

    private Vector3 originalScale;
    private float scrollAmount = 0;
    private float seePlayerZoom = 0;
    private float lastPlayerSeeRefresh = 0;
    private Renderer playerRenderer;

    private WillRenderPlayer willRenderPlayer;

    void Start()
    {
        originalScale = cameraOrbit.transform.localScale;

        playerRenderer = player_head.GetComponent<Renderer>();
        willRenderPlayer = player_head.GetComponent<WillRenderPlayer>();
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

        if (scrollAmount != 0 || seePlayerZoom != 0)
            cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - (scrollAmount + seePlayerZoom));

        if (!CanSeePlayer())
        {
            if ((scrollAmount + seePlayerZoom) < 0.7f)
            {
                seePlayerZoom += 0.1f;
            }
        }
        //if (Input.GetKeyUp("f"))
        //    CanSeePlayer();
    }

    public void SimulateMouse(float h, float v)
    {
        if (cameraOrbit.transform.eulerAngles.z + v <= 0.1f || cameraOrbit.transform.eulerAngles.z + v >= 179.9f)
            v = 0;

        cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + h, cameraOrbit.transform.eulerAngles.z + v);
    }

    bool CanSeePlayer()
    {
        int layerMask = 1 << 2;// Bit shift the index of the layer (8) to get a bit mask
                               // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(cam.transform.position, (player.transform.position - cam.transform.position), 100, layerMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //Debug.Log("i: " + i + " Name: " + hit.transform.gameObject.name + " Tag: " + hit.transform.gameObject.tag);
            switch(hit.transform.gameObject.tag)
            {
                case "PlayerCameraIgnore":
                case "MainCamera":
                case "Weapon":
                    continue;
            }
            if (hit.transform.gameObject.tag == "Player") return true;
            if (hit.collider.isTrigger) continue;
            return false;
        }
        return false;
    }
}
