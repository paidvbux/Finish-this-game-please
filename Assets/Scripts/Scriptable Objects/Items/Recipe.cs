using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe/Recipe")]
public class Recipe : ScriptableObject
{
    #region Classes
    public class RecipeItem
    {
        public Item item;
        public int amountRequired;
    }
    #endregion

    #region General Settings
    [Header("General Settings")]
    public RecipeItem result;
    public RecipeItem[] requiredItems;
    #endregion
}
