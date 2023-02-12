using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPotScript : CraftingStationScript
{
    #region Cooking Pot Settings
    [Header("Cooking Pot Settings")]
    [SerializeField] GameObject potLiquid;
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
}
