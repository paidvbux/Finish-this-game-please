using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe/Recipe")]
public class Recipe : ScriptableObject
{
    #region Classes
    [System.Serializable]
    public class RecipeItem
    {
        string name => item.name;

        public Item item;
        public int amount;

        public RecipeItem(Item _item, int _amountRequired)
        {
            item = _item;
            amount = _amountRequired;
        }
    }
    #endregion

    #region General Settings
    [Header("General Settings")]
    public List<RecipeItem> requiredItems;
    public RecipeItem result;
    #endregion
}
