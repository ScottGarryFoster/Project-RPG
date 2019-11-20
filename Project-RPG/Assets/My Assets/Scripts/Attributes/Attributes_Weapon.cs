using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes_Weapon : MonoBehaviour
{
    [Header("Trees")]
    [Tooltip("Can we get wood from trees")]
    public bool CanChopTrees;
    [Tooltip("Can we get wood from Oak")]
    public bool CanChopOak;
    [Tooltip("Multiplier for Wood")]
    public float OakMultiplier = 1;
}
