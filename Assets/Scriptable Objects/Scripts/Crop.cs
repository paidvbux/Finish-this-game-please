using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Scriptable Objects/Crop")]
public class Crop : Item
{
    [Header("Crop Settings")]
    public float cropGrowTime = 30.0f;
}
