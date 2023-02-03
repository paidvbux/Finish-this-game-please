using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyStationScript : StationScript
{
    #region Trigger Areas
    [Header("Trigger Areas")]
    public TriggerScript playerTrigger;
    #endregion

    #region Buy Station Variables/Settings
    [Header("Buy Station Settings")]
    public Dictionary<Item, int> cart;
    public List<ShopItemScript> shopItems;
    public GameObject boxPrefab;
    #endregion

    #region Hidden Variables
    Item selectedItem;
    int amountToAdd;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public override void Interact()
    {
        LoadShopUI();
    }

    void LoadShopUI()
    {
        GameManager.ToggleCursor(true);
        GameManager.shopUI.SetActive(true);
        UpdateUIButtons(selectedItem);
    }

    void UpdateUIButtons(Item item)
    {
        bool canAffordIncrement = GetTotal() + (amountToAdd * item.buyCost) <= GameManager.singleton.coins;

        GameManager.shopDecreaseButton.interactable = !cart.ContainsKey(item);
        GameManager.shopIncreaseButton.interactable = canAffordIncrement;
    }

    public void AddToCart(Item item)
    {
        if (cart.ContainsKey(item))
            cart[item] += amountToAdd;
        else if (!cart.ContainsKey(item))
            cart.Add(item, amountToAdd);
    }

    int GetTotal()
    {
        int total = 0;
        foreach (KeyValuePair<Item, int> value in cart)
        {
            int cost = value.Value * value.Key.buyCost;
            total += cost;
        }

        return total;
    }
    #endregion
}
