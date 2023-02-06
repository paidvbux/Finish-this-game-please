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
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void UpdateUI()
    {
        nameText.text = item.name;
        costText.text = item.buyCost.ToString();

        if (item.sprite != null)
            image.sprite = item.sprite;
    }

    public void UpdateCurrentItem()
    {
        GameManager.UpdateSelectedItem(item);
    }
    #endregion
}
