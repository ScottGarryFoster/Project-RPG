using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage_BuildingOrNoPass : MonoBehaviour
{
    //Public Var
    [Header("Outside AI")]
    [Tooltip("List all the buildings the outside AI shouldn't move into")]
    public Collider[] OutsideBuildings;
}
