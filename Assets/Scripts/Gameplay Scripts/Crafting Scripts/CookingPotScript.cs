using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPotScript : CraftingStationScript
{
    #region Cooking Pot Settings
    [Header("Cooking Pot Settings")]
    [SerializeField] Material failedMaterial;
    [SerializeField] MeshRenderer potLiquidMeshRenderer;
    [SerializeField] Transform potLiquid;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        potLiquid.localScale = new Vector3(1, 0, 1);
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
            potLiquid.localScale = new Vector3(1, Mathf.Lerp(0, 1, (cookTime - timer) / cookTime), 1);
            if (timer <= 0)
                CookFinish();
        }
    }
    #endregion

    #region Custom Functions
    protected override void Craft()
    {
        base.Craft();

        if (selectedRecipe == null)
            potLiquidMeshRenderer.material = failedMaterial;
        else
            potLiquidMeshRenderer.material = selectedRecipe.craftingMaterial;
    }

    protected override void OnClear()
    {
        doneCooking = false;

        potLiquid.localScale = new Vector3(1, 0, 1);

        Destroy(outline);
        outline = null;
    }
    #endregion
}
