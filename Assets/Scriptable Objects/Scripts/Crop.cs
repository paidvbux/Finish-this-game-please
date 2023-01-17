using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Scriptable Objects/Crop")]
public class Crop : ScriptableObject
{
    [Header("General Settings")]
    public string name;

    [Header("Display Settings")]
    public GameObject grabbableCropObject;
    public GameObject cropObject;

    [Header("Harvest Settings")]
    public float cropGrowTime = 30.0f;
    public int minHarvestCount;
    public int maxHarvestCount;

    [Header("Storage Settings")]
    public float mass;
    public int maxCrateStorage;
}
