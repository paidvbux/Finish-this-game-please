using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class RecipeBook : MonoBehaviour
{
    #region Static Variables
    public static RecipeBook singleton;

    public static List<Recipe> unlockedRecipes;
    public static List<Recipe> lockedRecipes;
    #endregion

    #region Recipe Book
    [Header("Recipe Book Settings")]
    [SerializeField] Image resultImage;
    [SerializeField] TextMeshProUGUI resultNameText;
    [SerializeField] TextMeshProUGUI resultAmountText;

    [Space()]
    [SerializeField] TextMeshProUGUI recipeNameText;
    [SerializeField] TextMeshProUGUI recipeDescriptionText;

    [Space()]
    public Transform recipeUIParent;
    public GameObject recipeUIPrefab;

    [Space()]
    public Transform recipeRequirementParent;
    public GameObject recipeRequirementUI;
    #endregion

    #region Hidden Variables
    [HideInInspector] public List<RecipeItemUIScript> loadedRecipeItems;

    List<GameObject> loadedRecipeObjects;
    #endregion

    //////////////////////////////////////////////////

    #region Unity Runtime Functions
    void Awake()
    {
        singleton = this;

        loadedRecipeObjects = new List<GameObject>();

        unlockedRecipes = new List<Recipe>();
    }
    #endregion

    #region Recipe Book Functions
    static void UnlockRecipe(Recipe recipe)
    {
        unlockedRecipes.Add(recipe);
    }

    public void LoadRecipeResult(Recipe.RecipeItem result)
    {
        recipeNameText.text = result.item.name;
        recipeDescriptionText.text = result.item.description;

        resultImage.sprite = result.item.sprite;
        resultNameText.text = result.item.name;
        resultAmountText.text = $"x{result.amount}";
    }

    public void LoadRecipeBook()
    {
        GameManager.singleton.menuUI.SetActive(false);

        foreach (GameObject loadedRecipeObject in loadedRecipeObjects)
            Destroy(loadedRecipeObject);

        loadedRecipeObjects.Clear();

        foreach (Recipe recipe in unlockedRecipes)
        {
            RecipeUIScript recipeUI = Instantiate(recipeUIPrefab, recipeUIParent.position, Quaternion.identity).GetComponent<RecipeUIScript>();
            recipeUI.LoadUI(recipe);

            loadedRecipeObjects.Add(recipeUI.gameObject);

            recipeUI.transform.SetParent(recipeUIParent);
        }

        RectTransform rect = recipeUIParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, loadedRecipeObjects.Count * 200);
    }
    #endregion

}