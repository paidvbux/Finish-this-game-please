using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStationScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    [SerializeField] protected GameManager.OutlineParams outlineParams;
    [SerializeField] protected string cookInteractText = "Cook";
    [SerializeField] protected string finishInteractText = "Pickup";
    [SerializeField] protected GameObject interactArea;
    [SerializeField] protected Transform itemSpawnPosition;
    [SerializeField] protected IngredientHolderScript[] itemSlots;
    [SerializeField] protected Item failedRecipeResult;
    [SerializeField] protected float failedCookingTime;
    #endregion

    #region Recipe Variables/Settings
    [Header("Recipe Settings")]
    [SerializeField] protected string recipesPath = "Recipes/";
    protected Recipe[] recipes;
    #endregion

    #region Hidden Variables
    protected Outline outline;

    protected List<Recipe.RecipeItem> storedRecipeItems = new List<Recipe.RecipeItem>();
    protected Recipe selectedRecipe;
    protected float cookTime;
    protected float timer;
    protected bool cooking;
    protected bool doneCooking;

    protected int amountToInstantiate;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        recipes = Resources.LoadAll<Recipe>(recipesPath);
    }

    void Update()
    {
        CheckIfInteractable();

        if (GameManager.isInteractableObject(interactArea) && Input.GetKeyDown(KeyCode.E))
        {
            if (doneCooking)
                FinishCooking();
            else if (!doneCooking && !cooking)
                Craft();
        }

        if (cooking)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                CookFinish();
        }
    }
    #endregion

    #region Custom Functions
    protected void CheckIfInteractable()
    {
        if (HoverScript.selectedGameObject == interactArea)
        {
            if (doneCooking)
            {
                if ((GameManager.isEmpty() || GameManager.interactableObject.text == cookInteractText) && GameManager.interactableObject.text != finishInteractText)
                    GameManager.SetInteractableObject(finishInteractText, interactArea);
            }

            if (!cooking && !doneCooking && GetCount() != 0)
            {
                if ((GameManager.isEmpty() || GameManager.interactableObject.text == finishInteractText) && GameManager.interactableObject.text != cookInteractText)
                    GameManager.SetInteractableObject(cookInteractText, interactArea);
            }

            if (cooking && GameManager.isInteractableObject(interactArea))
                GameManager.SetInteractableObject();
        }
        if (HoverScript.selectedGameObject != interactArea && GameManager.isInteractableObject(interactArea))
            GameManager.SetInteractableObject();
    }

    protected virtual void Craft()
    {
        Dictionary<Item, int> storedItems = LoadItems();
        storedRecipeItems = LoadRecipeItems(storedItems);

        if (storedRecipeItems.Count == 0)
            return;

        selectedRecipe = CheckForRecipe();

        if (selectedRecipe == null)
            amountToInstantiate = GetCount();
        else
            amountToInstantiate = selectedRecipe.result.amount;

        cookTime = selectedRecipe == null ? (failedCookingTime * GetCount()) : selectedRecipe.cookingTime;

        foreach (IngredientHolderScript itemSlot in itemSlots)
            itemSlot.Clear();

        timer = cookTime;

        cooking = true;
    }

    protected void SummonItem(Item itemToSummmon)
    {
        GameObject instantiatedItem = Instantiate(itemToSummmon.grabbableObject, itemSpawnPosition.position, Quaternion.identity);
        instantiatedItem.transform.SetParent(GameManager.singleton.transform);
    }

    protected virtual void OnClear()
    {
        doneCooking = false; 

        Destroy(outline);
        outline = null;
    }

    protected void FinishCooking()
    {
        if (selectedRecipe == null)
            SummonItem(failedRecipeResult);
        else
            SummonItem(selectedRecipe.result.item);

        if (--amountToInstantiate == 0)
        {
            OnClear();
            return;
        }
    }

    protected void CookFinish()
    {
        cooking = false;
        doneCooking = true;

        outline = GameManager.AddOutline(interactArea, outlineParams);
    }

    #region Helper Functions
    protected int GetCount()
    {
        int totalAmount = 0;
        foreach (IngredientHolderScript itemSlot in itemSlots)
            totalAmount += itemSlot.storedItem != null ? 1 : 0;

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
