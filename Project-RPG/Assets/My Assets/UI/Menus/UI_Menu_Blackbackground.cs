using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu_Blackbackground : MonoBehaviour
{
    public float Opacity = 0.5f;
    private bool triggeredFade = false;

    private RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = gameObject.GetComponent<RawImage>();
        SetAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.active)
            SetAlpha(0);
        else if(!triggeredFade)
            SetAlpha(Opacity);
    }

    void SetAlpha(float opacity)
    {
        float alpha = 1.0f; //1 is opaque, 0 is transparent
        Color currColor = rawImage.color;
        currColor.a = opacity;
        rawImage.color = currColor;
    }

    void SetAlpha(float opacity, float time)
    {
        rawImage.CrossFadeAlpha(opacity, time, true);
    }
}
