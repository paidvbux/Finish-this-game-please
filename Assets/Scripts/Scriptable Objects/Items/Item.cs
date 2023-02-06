using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string itemName;
    public string pluralItemName;
    #endregion

    #region Display Variables/Settings
    [Header("Display Settings")]
    public GameObject grabbableObject;
    public GameObject gameObject;
    public Sprite sprite;
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
    public string description;
    #endregion

    #region Quest Variables/Settings
    [Header("Quest Settings")]
    public int minQuestRequirement;
    public int maxQuestRequirement;
    #endregion
}
