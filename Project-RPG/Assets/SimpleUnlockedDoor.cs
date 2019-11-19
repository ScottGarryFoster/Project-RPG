using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUnlockedDoor : MonoBehaviour
{
    //Public Var
    [Header("Basic Setup Settings")]
    [Tooltip("If true, door is locked.")]
    public bool Locked;
    [Tooltip("If the player is inside the collider you can interact, this is the collider")]
    public bool InteractionCollider;
    [Tooltip("What tag does the object inside need to have to interact")]
    public string InteractionTag = "Player";

    [Header("DisableColliders")]
    [Tooltip("If you want colliders to be disabled when the door opens/closed drag them here")]
    public Collider[] DisableColliders;

    [Header("Advanced Settings")]
    [Tooltip("How fast do you want it to open or close")]
    [Range(1,20)]
    public float MovementSpeed = 5.0f;
    [Tooltip("What axis should the door rotate around")]
    public char RotationAxis = 'y';
    [Tooltip("What rotation number is closed on the axis?")]
    public float ClosedRotationState = 0;
    [Tooltip("What rotation number is open on the axis?")]
    public float OpenRotationState = 90;
    [Tooltip("Override what the transform will rotate")]
    public Transform RotationTransform;

    [Header("Autoclose")]
    [Tooltip("If true the door will automatically close after so long")]
    public bool Autoclose = true;
    [Tooltip("How soon (in seconds) should the door autoclose?")]
    [Range(1, 120)]
    public int AutocloseTime = 20;

    //Private
    private bool AreMoving = false;
    private float DestinationRotation = 0;
    private bool DestinationDoorState = false;
    private bool CurrentState = false; //True - open
    private bool DirectionToRotate = false; //False - Add, True - Subtract

    private float CurrentAutoCloseCountdown = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (RotationTransform == null) RotationTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Autoclose) AutoClose();
        if (AreMoving) OpenCloseMovement();
    }

    public void OpenDoor()
    {
        if (CurrentState) return;
        AreMoving = true;
        DestinationRotation = OpenRotationState;
        DestinationDoorState = true;
    }

    public void ClosedDoor()
    {
        if (!CurrentState) return;
        AreMoving = true;
        DestinationRotation = ClosedRotationState;
        DestinationDoorState = false;
    }

    public void FlipDoorState()
    {
        if (CurrentState)
            ClosedDoor();
        else
            OpenDoor();
    }

    void AutoClose()
    {
        if (AreMoving) return;
        if (!CurrentState) return;
        CurrentAutoCloseCountdown += Time.deltaTime;
        if (CurrentAutoCloseCountdown <= AutocloseTime) return;
        AreMoving = true;
        DestinationRotation = ClosedRotationState;
        DestinationDoorState = false;
        CurrentAutoCloseCountdown = 0;
    }

    void OpenCloseMovement()
    {
        FlipColliders(false);

        Vector3 endPoint = new Vector3(0, 0, 0);

        float l_speedAdjusted = MovementSpeed * Time.deltaTime;
        switch (RotationAxis)
        {
            case 'x':
                endPoint.x = DestinationRotation;
                break;
            case 'z':
                endPoint.z = DestinationRotation;
                break;
            default:
            case 'y':
                endPoint.y = DestinationRotation;
                break;
        }
        Quaternion target = Quaternion.Euler(endPoint.x, endPoint.y, endPoint.z);

        RotationTransform.rotation = Quaternion.Slerp(RotationTransform.rotation, target, Time.deltaTime * MovementSpeed);// Dampen towards the target rotation

        switch (RotationAxis)
        {
            case 'x':
                if (DifforenceBetweenTwoFloats(DestinationRotation, RotationTransform.eulerAngles.x) <= 0.5f)
                    ResetMovement();
                break;
            case 'z':
                if (DifforenceBetweenTwoFloats(DestinationRotation, RotationTransform.eulerAngles.z) <= 0.5f)
                    ResetMovement();
                break;
            default:
            case 'y':
                if (DifforenceBetweenTwoFloats(DestinationRotation, RotationTransform.eulerAngles.y) <= 0.5f)
                    ResetMovement();
                break;
        }
    }

    void ResetMovement()
    {
        Debug.Log("Transitioning");
        AreMoving = false;
        CurrentState = DestinationDoorState;
        FlipColliders(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != InteractionTag) return;
        if (Input.GetKeyUp("e"))
            FlipDoorState();
    }

    float DifforenceBetweenTwoFloats(float a, float b)
    {
        if (a > b)
            return a - b;
        else
            return b - a;
    }

    void FlipColliders(bool point)
    {
        if (DisableColliders.Length <= 0) return;
        for(int i = 0; i < DisableColliders.Length; i++)
        {
            Collider coll = DisableColliders[i];
            coll.enabled = point;
        }
    }
}
