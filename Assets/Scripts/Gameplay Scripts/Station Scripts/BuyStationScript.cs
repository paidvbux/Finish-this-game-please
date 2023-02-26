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
    public Transform boxSpawnPosition;
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
        cartItems = new Dictionary<Item, CartItemScript>();
    }
    #endregion

    #region Custom Functions
    #region Public Functions
    public void Checkout()
    {
        if (cart.Count == 0)
            return;

        GameManager.singleton.coins -= GetTotal();

        ContainerScript containerScript = Instantiate(boxPrefab, boxSpawnPosition).GetComponent<ContainerScript>();
        containerScript.storedItems = new List<ContainerScript.StoredItem>();

        foreach (KeyValuePair<Item, int> item in cart)
            containerScript.storedItems.Add(new ContainerScript.StoredItem(item.Key, item.Value, item.Key.isSeedPacket));
    }
    
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
        {
            ShopScript.singleton.loadedCartItems.Remove(cartItems[selectedItem].gameObject);

            cart.Remove(selectedItem);
            Destroy(cartItems[selectedItem].gameObject);
            cartItems.Remove(selectedItem);

        }
        UpdateUIButtons();
        UpdateCartUI();
        UpdateItemUI();
    }

    public void AddToCart()
    {
        if (cart.ContainsKey(selectedItem))
            cart[selectedItem] += changeAmount;
        else if (!cart.ContainsKey(selectedItem))
        {
            cart.Add(selectedItem, changeAmount);

            CartItemScript cartItem = Instantiate(ShopScript.singleton.cartItemUI, ShopScript.singleton.cartItemUIParent).GetComponent<CartItemScript>();
            cartItem.item = selectedItem;
            cartItem.amount = changeAmount;

            cartItems.Add(selectedItem, cartItem);

            ShopScript.singleton.loadedCartItems.Add(cartItem.gameObject);

            RectTransform rect = ShopScript.singleton.cartItemUIParent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 150 * ShopScript.singleton.loadedCartItems.Count);
        }

        UpdateUIButtons();
        UpdateCartUI();
        UpdateItemUI();
    }

    public override void Interact()
    {
        if (GameManager.uiActive) 
            return;
        ShopScript.singleton.selectedShop = this;
        LoadShopUI();
    }

    public void LoadCartUI()
    {
        UpdateCartUI();
        ShopScript.singleton.cartUI.SetActive(true);
    }
    #endregion

    void UpdateCartUI()
    {
        int total = GetTotal();
        foreach (KeyValuePair<Item, CartItemScript> cartItem in cartItems)
        {
            cartItem.Value.amount = cart[cartItem.Key];
            cartItem.Value.UpdateUI();
        }

        ShopScript.singleton.totalText.text = $"{total}";
        ShopScript.singleton.currentBalanceText.text = $"{GameManager.singleton.coins}";
        ShopScript.singleton.remainingBalanceText.text = $"{GameManager.singleton.coins - total}";
    }

    void UpdateItemUI()
    {
        string amount = (cart.ContainsKey(selectedItem) ? cart[selectedItem] : 0).ToString();

        ShopScript.singleton.shopNameText.text = $"{selectedItem.itemName}";
        ShopScript.singleton.shopDescriptionText.text = $"{selectedItem.description}";
        ShopScript.singleton.shopAmountText.text = $"x{amount}";
        ShopScript.singleton.shopCostText.text = $"{selectedItem.buyCost}";
        ShopScript.singleton.shopIncrementText.text = $"{changeAmount}";
    }

    void LoadShopUI()
    {
        GameManager.uiActive = true;
        GameManager.ToggleCursor(true);

        foreach(GameObject loadedShopItem in ShopScript.singleton.loadedShopItems)
            Destroy(loadedShopItem);
        ShopScript.singleton.loadedShopItems.Clear();

        foreach (Item item in purchasableItems)
        {
            ShopItemScript shopItem = Instantiate(ShopScript.singleton.itemUI, ShopScript.singleton.itemUIParent).GetComponent<ShopItemScript>();
            shopItem.item = item;
            shopItem.UpdateUI();

            ShopScript.singleton.loadedShopItems.Add(shopItem.gameObject);

            shopItems.Add(shopItem);
        }

        RectTransform rect = ShopScript.singleton.itemUIParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 150 * ShopScript.singleton.loadedShopItems.Count);

        ShopScript.singleton.shopUI.SetActive(true);
    }

    void UpdateUIButtons()
    {
        bool canAffordIncrement = GetTotal() + (changeAmount * selectedItem.buyCost) <= GameManager.singleton.coins;

        ShopScript.singleton.shopIncreaseButton.interactable = canAffordIncrement;
        ShopScript.singleton.shopDecreaseButton.interactable = cart.ContainsKey(selectedItem);
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
