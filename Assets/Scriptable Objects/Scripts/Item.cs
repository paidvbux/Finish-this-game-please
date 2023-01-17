using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("General Settings")]
    public string name;

    [Header("Display Settings")]
    public GameObject grabbableCropObject;
    public GameObject cropObject;

    [Header("Harvest Settings")]
    public int minHarvestCount;
    public int maxHarvestCount;

    [Header("Storage Settings")]
    public float mass;
    public int maxCrateStorage;

    [Header("Buy/Sell Settings")]
    public int buyCost;
    public int sellCost;
}
