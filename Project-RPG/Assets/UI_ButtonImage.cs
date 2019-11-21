using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonImage : MonoBehaviour
    , IPointerClickHandler // 2
     , IDragHandler
     , IPointerEnterHandler
     , IPointerExitHandler
    , IPointerDownHandler
    , IPointerUpHandler
{
    [Header("UV Rect Setup")]
    [Tooltip("No Mouse Over or Click")]
    public Rect RectMouseNeutral;
    [Tooltip("Mouse Over when not clicking")]
    public Rect RectMouseOver;
    [Tooltip("Mouse is currently being clicked and the cursor is over the button")]
    public Rect RectMouseDown;
    [Tooltip("Mouse has been clicked. This will appear for a set amount of time after a click.")]
    public Rect RectMouseClick;
    [Tooltip("How long after a click should the clicked version be shown.")]
    public float MouseClickCooldownTimer = 0.15f;


    private RawImage rawImage;
    private float MouseClickCooldown = 0;
    private bool MouseIsOverCursor = false;
    private bool MouseClick = false;

    // Start is called before the first frame update
    void Start()
    {
        rawImage = gameObject.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateImage();
    }

    void UpdateImage()
    {
        if (MouseClickCooldown > 0)
        {
            rawImage.uvRect = RectMouseClick;
            MouseClickCooldown -= Time.deltaTime;
            if (MouseClickCooldown <= 0) MouseClickCooldown = 0;
            return;
        }

        if (MouseClick)
            rawImage.uvRect = RectMouseDown;
        else
        {
            if (MouseIsOverCursor)
                rawImage.uvRect = RectMouseOver;
            else
                rawImage.uvRect = RectMouseNeutral;
        }
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        //MouseClick = true;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        MouseClick = true;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (MouseClick) MouseClickCooldown = MouseClickCooldownTimer;
        MouseClick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseIsOverCursor = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseIsOverCursor = false;
        if (MouseClick) MouseClickCooldown = MouseClickCooldownTimer;
        MouseClick = false;
    }
}
