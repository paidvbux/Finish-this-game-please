using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RecipeUIScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    public TextMeshProUGUI recipeName;
    public Image recipeImage;
    #endregion

    #region Hidden Variables
    [HideInInspector] public List<Recipe.RecipeItem> itemsRequired;
    [HideInInspector] public Recipe.RecipeItem itemResult;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void LoadUI(Recipe recipe)
    {
        itemsRequired = recipe.requiredItems;
        itemResult = recipe.result;

        recipeName.text = recipe.name;
        recipeImage.sprite = recipe.result.item.sprite;
    }

    public void LoadRecipeUI()
    {
        foreach (RecipeItemUIScript loadedRecipeItem in GameManager.singleton.loadedRecipeItems)
            Destroy(loadedRecipeItem.gameObject);
        GameManager.singleton.loadedRecipeItems.Clear();

        foreach (Recipe.RecipeItem recipeItem in itemsRequired)
        {
            RecipeItemUIScript recipeUI = Instantiate(GameManager.recipeRequirementUI, GameManager.recipeRequirementParent).GetComponent<RecipeItemUIScript>();
            recipeUI.item = recipeItem;

            recipeUI.LoadItem();
        }
    }
    #endregion
}
