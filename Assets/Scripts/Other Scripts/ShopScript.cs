using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopScript : MonoBehaviour
{
    #region Static Variables
    public static ShopScript singleton;
    #endregion

    #region Shop UI
    [Header("Shop UI Settings")]
    public GameObject shopUI;
    public GameObject cartUI;
    public Transform itemUIParent;
    public GameObject itemUI;
    public Transform cartItemUIParent;
    public GameObject cartItemUI;
    #endregion

    #region Shop Description UI
    [Header("Shop Description UI Settings")]
    public GameObject shopDescriptionUI;
    public TextMeshProUGUI shopNameText;
    public TextMeshProUGUI shopCostText;
    public TextMeshProUGUI shopDescriptionText;
    public TextMeshProUGUI shopAmountText;
    public TextMeshProUGUI shopIncrementText;
    public TMP_InputField incrementInputField;
    public Button shopIncreaseButton;
    public Button shopDecreaseButton;
    #endregion

    #region Cart UI
    [Header("Cart UI Settings")]
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI currentBalanceText;
    public TextMeshProUGUI remainingBalanceText;
    #endregion

    #region Hidden Variables
    [HideInInspector] public BuyStationScript selectedShop;
    [HideInInspector] public List<GameObject> loadedShopItems;
    [HideInInspector] public List<GameObject> loadedCartItems;
    #endregion

    //////////////////////////////////////////////////

    #region Unity Runtime Functions
    void Awake()
    {
        singleton = this;

        loadedShopItems = new List<GameObject>();
        loadedCartItems = new List<GameObject>();

        DisableShopUI();
    }

    void Update()
    {
        #region Update Shop UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableShopUI();
        }
        #endregion
    }
    #endregion

    #region Custom Functions
    public void SubmitIncrementAmount(TMP_InputField inputField)
    {
        string stringInput = string.IsNullOrEmpty(inputField.text) ? "1" : inputField.text;
        int maxValue = Mathf.FloorToInt((GameManager.singleton.coins / selectedShop.selectedItem.buyCost) -
            (selectedShop.cart.ContainsKey(selectedShop.selectedItem) ? selectedShop.cart[selectedShop.selectedItem] : 0));
        int intInput = Mathf.Clamp(int.Parse(stringInput), 1, maxValue);

        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = intInput.ToString();
        inputField.text = intInput.ToString();

        selectedShop.SetIncrement(intInput);
    }

    public static void UpdateSelectedItem(Item item)
    {
        singleton.shopDescriptionUI.SetActive(true);
        singleton.selectedShop.UpdateSelectedItem(item);
    }

    public void AddToCart()
    {
        selectedShop.AddToCart();
    }

    public void RemoveFromCart()
    {
        selectedShop.RemoveFromCart();
    }

    public void Checkout()
    {
        selectedShop.Checkout();
    }

    #region Shop UI
    public void DisableShopUI()
    {
        if (selectedShop != null)
        {
            foreach (ShopItemScript shopItem in selectedShop.shopItems)
                Destroy(shopItem.gameObject);

            selectedShop.shopItems.Clear();
            selectedShop.cart.Clear();
        }

        DisableCartUI();

        shopUI.SetActive(false);
        shopDescriptionUI.SetActive(false);
        GameManager.ToggleCursor(false);
    }

    public void LoadCartUI()
    {
        selectedShop.LoadCartUI();
    }

    public void DisableCartUI()
    {
        if (selectedShop != null)
        {
            foreach (KeyValuePair<Item, CartItemScript> cartItem in selectedShop.cartItems)
                Destroy(cartItem.Value.gameObject);

            selectedShop.cartItems.Clear();
        }

        cartUI.SetActive(false);
    }
    #endregion
    #endregion
}
