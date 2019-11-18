using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerPos;

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale.x <= 1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        if (this.transform.localScale.x >= 15)
        {
            this.transform.localScale = new Vector3(15, 15, 15);
        }

        playerPos = player.transform.position;
        this.transform.position = playerPos;
    }


}
