using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStationScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    [SerializeField] protected string interactText;
    [SerializeField] protected GameObject interactArea;
    [SerializeField] protected Transform itemSpawnPosition;
    [SerializeField] protected IngredientHolderScript[] itemSlots;
    [SerializeField] protected Item failedRecipeResult;
    [SerializeField] protected int failedCookingTime;
    #endregion

    #region Recipe Variables/Settings
    [Header("Recipe Settings")]
    [SerializeField] protected string recipesPath = "Recipes/";
    protected Recipe[] recipes;
    #endregion

    #region Hidden Variables
    protected List<Recipe.RecipeItem> storedRecipeItems = new List<Recipe.RecipeItem>();
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        recipes = Resources.LoadAll<Recipe>(recipesPath);
    }

    void Update()
    {
        GameManager.CheckIfInteractable(interactText, interactArea);

        if (GameManager.isInteractableObject(interactArea) && Input.GetKeyDown(KeyCode.E))
            Craft();
    }
    #endregion

    #region Custom Functions
    protected void Craft()
    {
        Dictionary<Item, int> storedItems = LoadItems();
        storedRecipeItems = LoadRecipeItems(storedItems);

        if (storedRecipeItems.Count == 0)
            return;

        Recipe selectedRecipe = CheckForRecipe();

        foreach (IngredientHolderScript itemSlot in itemSlots)
            itemSlot.Clear();

        WaitForCookFinish(selectedRecipe);
    }

    protected void SummonItem(Item itemToSummmon, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject instantiatedItem = Instantiate(itemToSummmon.grabbableObject, itemSpawnPosition.position, Quaternion.identity);
            instantiatedItem.transform.SetParent(GameManager.singleton.transform);
        }
    }

    protected void WaitForCookFinish(Recipe recipe)
    {
        float cookTime = recipe == null ? failedCookingTime : recipe.cookingTime;
        float timer = cookTime;

        //Add Scale lerp.

        if (recipe == null)
            SummonItem(failedRecipeResult, GetCount());
        else
            SummonItem(recipe.result.item, recipe.result.amount);
    }

    #region Helper Functions
    protected int GetCount()
    {
        int totalAmount = 0;
        foreach (Recipe.RecipeItem storedRecipeItem in storedRecipeItems)
            totalAmount += storedRecipeItem.amount;

        return totalAmount;
    }

    protected Recipe CheckForRecipe()
    {
        Recipe selectedRecipe = null;

        foreach (Recipe recipe in recipes)
        {
            bool isRecipe = true;
            for (int i = 0; i < recipe.requiredItems.Count; i++)
            {
                bool notNull = storedRecipeItems[i] != null && recipe.requiredItems[i] != null;
                bool notEqual = recipe.requiredItems[i].amount != storedRecipeItems[i].amount || recipe.requiredItems[i].item != storedRecipeItems[i].item;
                if (notNull && notEqual)
                {
                    isRecipe = false;
                    break;
                }
            }

            if (isRecipe)
                selectedRecipe = recipe;
        }

        return selectedRecipe;
    }

    protected Dictionary<Item, int> LoadItems()
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

    protected List<Recipe.RecipeItem> LoadRecipeItems(Dictionary<Item, int> items)
    {
        List<Recipe.RecipeItem> recipeItems = new List<Recipe.RecipeItem>();

        foreach (KeyValuePair<Item, int> item in items)
            recipeItems.Add(new Recipe.RecipeItem(item.Key, item.Value));

        return recipeItems;
    }
    #endregion
    #endregion
}
