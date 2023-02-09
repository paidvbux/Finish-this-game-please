using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe/Recipe")]
public class Recipe : ScriptableObject
{
    #region General Settings
    [Header("General Settings")]
    public Item result;
    public Item[] requiredItems;
    #endregion
}
