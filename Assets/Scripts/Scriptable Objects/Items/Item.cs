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

    public bool isSeedPacket;
    #endregion

    #region Display Variables/Settings
    [Header("Display Settings")]
    public GameObject grabbableObject;
    public Sprite sprite;
    #endregion

    #region Crafting Variables/Settings
    [Header("Crafting Settings")]
    public Vector3 placedRotation;
    public Vector3 placedOffset;
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
}
