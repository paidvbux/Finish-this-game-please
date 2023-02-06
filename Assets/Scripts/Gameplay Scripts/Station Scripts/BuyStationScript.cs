using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyStationScript : StationScript
{
    #region Buy Station Variables/Settings
    [Header("Buy Station Settings")]
    public List<Item> purchasableItems;
    public Dictionary<Item, int> cart;
    public GameObject boxPrefab;
    #endregion

    #region Hidden Variables
    [HideInInspector] public List<ShopItemScript> shopItems;
    [HideInInspector] public Dictionary<Item, CartItemScript> cartItems;
    [HideInInspector] public Item selectedItem;
    int changeAmount;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        changeAmount = 1;

        cart = new Dictionary<Item, int>();
    }
    #endregion

    #region Custom Functions
    #region Public Functions
    public void SetIncrement(int value)
    {
        changeAmount = value;
        UpdateUIButtons();
    }

    public void UpdateSelectedItem(Item item)
    {
        selectedItem = item;
        UpdateItemUI();
    }

    public void RemoveFromCart()
    {
        if (cart.ContainsKey(selectedItem) && cart[selectedItem] > changeAmount)
            cart[selectedItem] -= changeAmount;
        else if (cart[selectedItem] <= changeAmount)
            cart.Remove(selectedItem);
        UpdateUIButtons();
        UpdateCartUI();
        UpdateItemUI();
    }

    public void AddToCart()
    {
        if (cart.ContainsKey(selectedItem))
            cart[selectedItem] += changeAmount;
        else if (!cart.ContainsKey(selectedItem))
            cart.Add(selectedItem, changeAmount);
        UpdateUIButtons();
        UpdateCartUI();
        UpdateItemUI();
    }

    public override void Interact()
    {
        GameManager.singleton.selectedShop = this;
        LoadShopUI();
    }

    public void LoadCartUI()
    {
        foreach (KeyValuePair<Item, int> item in cart)
        {
            CartItemScript cartItem = Instantiate(GameManager.singleton.cartItemUI, GameManager.singleton.cartItemUIParent).GetComponent<CartItemScript>();
            cartItem.item = item.Key;
            cartItem.amount = item.Value;

            cartItems.Add(item.Key, cartItem);
        }

        UpdateCartUI();
        GameManager.cartUI.SetActive(true);
    }
    #endregion

    void UpdateCartUI()
    {
        int total = 0;
        foreach (KeyValuePair<Item, CartItemScript> cartItem in cartItems)
        {
            cartItem.Value.amount = cart[cartItem.Key];
            cartItem.Value.UpdateUI();

            total += cart[cartItem.Key] * cartItem.Key.buyCost;
        }

        GameManager.totalText.text = $"{total}";
        GameManager.currentBalanceText.text = $"{GameManager.singleton.coins}";
        GameManager.remainingBalanceText.text = $"{GameManager.singleton.coins - total}";
    }

    void UpdateItemUI()
    {
        string amount = (cart.ContainsKey(selectedItem) ? cart[selectedItem] : 0).ToString();

        GameManager.shopNameText.text = $"{selectedItem.itemName}";
        GameManager.shopDescriptionText.text = $"{selectedItem.description}";
        GameManager.shopAmountText.text = $"x{amount}";
        GameManager.shopCostText.text = $"{selectedItem.buyCost}";
        GameManager.shopIncrementText.text = $"{changeAmount}";
    }

    void LoadShopUI()
    {
        GameManager.uiActive = true;
        GameManager.ToggleCursor(true);
        foreach (Item item in purchasableItems)
        {
            ShopItemScript shopItem = Instantiate(GameManager.singleton.itemUI, GameManager.singleton.itemUIParent).GetComponent<ShopItemScript>();
            shopItem.item = item;
            shopItem.UpdateUI();

            shopItems.Add(shopItem);
        }

        GameManager.shopUI.SetActive(true);
    }

    void UpdateUIButtons()
    {
        bool canAffordIncrement = GetTotal() + (changeAmount * selectedItem.buyCost) <= GameManager.singleton.coins;

        GameManager.shopIncreaseButton.interactable = canAffordIncrement;
        GameManager.shopDecreaseButton.interactable = cart.ContainsKey(selectedItem);
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
