using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RecipeItemUIScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;
    #endregion
    
    #region Hidden Variables
    [HideInInspector] public Recipe.RecipeItem item;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void LoadItem()
    {
        itemImage.sprite = item.item.sprite; 

        nameText.text = item.item.name;
        amountText.text = $"x{item.amount}";
    }
    #endregion
}
