using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStationScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    #endregion

    #region General Settings
    [Header("General Settings")]
    [SerializeField] Transform itemSpawnPosition;
    [SerializeField] IngredientHolderScript[] itemSlots;
    [SerializeField] Item failedRecipeResult;
    #endregion

    #region Recipe Variables/Settings
    [Header("Recipe Settings")]
    [SerializeField] string recipesPath = "Recipes/";
    [SerializeField] Recipe[] recipes;
    #endregion

    #region Hidden Variables
    List<Recipe.RecipeItem> storedRecipeItems = new List<Recipe.RecipeItem>();
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        recipes = Resources.LoadAll<Recipe>(recipesPath);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Craft();
    }
    #endregion

    #region Custom Functions
    void Craft()
    {
        Dictionary<Item, int> storedItems = LoadItems();
        storedRecipeItems = LoadRecipeItems(storedItems);

        Recipe selectedRecipe = null;
        foreach (Recipe recipe in recipes)
        {
            if (!recipe.requiredItems.Equals(storedRecipeItems))
                continue;

            selectedRecipe = recipe;
        }

        if (selectedRecipe == null)
            SummonItem(failedRecipeResult, GetCount());
        else
            SummonItem(selectedRecipe.result.item, selectedRecipe.result.amount);

        foreach (IngredientHolderScript itemSlot in itemSlots)
            itemSlot.Clear();
    }

    void SummonItem(Item itemToSummmon, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject instantiatedItem = Instantiate(itemToSummmon.grabbableObject, itemSpawnPosition.position, Quaternion.identity);
            instantiatedItem.transform.SetParent(GameManager.singleton.transform);
        }
    }

    int GetCount()
    {
        int totalAmount = 0;
        foreach (Recipe.RecipeItem storedRecipeItem in storedRecipeItems)
            totalAmount += storedRecipeItem.amount;

        return totalAmount;
    }

    Dictionary<Item, int> LoadItems()
    {
        Dictionary<Item, int> items = new Dictionary<Item, int>();

        foreach (IngredientHolderScript itemSlot in itemSlots)
        {
            if (itemSlot.storedItem == null)
                continue;

            if (items.ContainsKey(itemSlot.storedItem))
                items[itemSlot.storedItem]++;
            else
                items.Add(itemSlot.storedItem, 1);
        }

        return items;
    }

    List<Recipe.RecipeItem> LoadRecipeItems(Dictionary<Item, int> items)
    {
        List<Recipe.RecipeItem> recipeItems = new List<Recipe.RecipeItem>();

        foreach (KeyValuePair<Item, int> item in items)
            recipeItems.Add(new Recipe.RecipeItem(item.Key, item.Value));

        return recipeItems;
    }
    #endregion
}
