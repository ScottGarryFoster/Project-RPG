using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    Camera cam;
    public GameObject player;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        /*if (Physics.Linecast(transform.position, player.transform.position))
        {
            Debug.Log("blocked");
        }*/

        //Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0.5f));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            Debug.Log("Hit: objectHit:" + objectHit.gameObject.name);
            // Do something with the object that was hit by the raycast.
        }

        /*RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector2(0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            Debug.Log("Hit: objectHit:" + objectHit.gameObject.name);
            // Do something with the object that was hit by the raycast.
        }*/
    }
}
