using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opt_Unrender : MonoBehaviour
{
    Renderer m_Renderer;
    // Use this for initialization
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Renderer.isVisible && !m_Renderer.enabled)
            m_Renderer.enabled = true;
        else if (!m_Renderer.isVisible && m_Renderer.enabled)
            m_Renderer.enabled = false;
    }
}
