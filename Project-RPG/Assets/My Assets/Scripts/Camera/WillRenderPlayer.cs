using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillRenderPlayer : MonoBehaviour
{
    public bool WillRender = false;

    void Update()
    {
        WillRender = false;
    }

    void OnWillRenderObject()
    {
        WillRender = true;
    }
}