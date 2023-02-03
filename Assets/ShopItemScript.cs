using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemScript : MonoBehaviour
{
    #region UI Variables/Settings
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Image image;
    #endregion

    #region Hidden Variables
    [HideInInspector] public Item item;
    [HideInInspector] public int amount;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void Setup(string name, int cost, Sprite sprite)
    {
        nameText.text = name;
        costText.text = cost.ToString();

        if (sprite != null)
            image.sprite = sprite;
    }

    public void UpdateCurrentItem()
    {
        GameManager.UpdateShopDescriptionUI(item, amount);
    }
    #endregion
}
