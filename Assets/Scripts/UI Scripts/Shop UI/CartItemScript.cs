using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartItemScript : MonoBehaviour
{
    #region UI Variables/Settings
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI costText;
    public Image image;
    #endregion

    #region Hidden Variables
    [HideInInspector] public Item item;
    [HideInInspector] public int amount;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void UpdateUI()
    {
        nameText.text = item.name;
        amountText.text = $"{amount}";
        costText.text = $"{item.buyCost * amount}";

        if (item.sprite != null)
            image.sprite = item.sprite;
    }

    public void UpdateCurrentItem()
    {
        ShopScript.UpdateSelectedItem(item);
    }
    #endregion
}
