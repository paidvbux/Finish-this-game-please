using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string itemName;
    #endregion

    #region Display Variables/Settings
    [Header("Display Settings")]
    public GameObject grabbableCropObject;
    public GameObject cropObject;
    #endregion

    #region Harvest Variables/Settings
    [Header("Harvest Settings")]
    public int minHarvestCount;
    public int maxHarvestCount;
    #endregion

    #region Storage Variables/Settings
    [Header("Storage Settings")]
    public float mass;
    public int maxCrateStorage;
    #endregion

    #region Buy/Sell Variables/Settings
    [Header("Buy/Sell Settings")]
    public int buyCost;
    public int sellCost;
    #endregion
}
