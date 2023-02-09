using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStationScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    #endregion

    #region Recipe Variables/Settings
    [Header("Recipe Settings")]
    [SerializeField] string recipesPath = "Recipes/";
    [SerializeField] Recipe[] recipes;
    #endregion
}
