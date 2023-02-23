using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Classes
    #region Other Classes
    [System.Serializable]
    public class OutlineParams
    {
        public Outline.Mode outlineMode = Outline.Mode.OutlineVisible;
        public Color color = Color.white;
        [Range(0, 10)] public float outlineWidth = 5;
    }

    [System.Serializable]
    public class InteractableObject
    {
        public string text;
        public GameObject gameObject;

        public InteractableObject(string _text, GameObject _gameObject)
        {
            text = _text;
            gameObject = _gameObject;
        }
    }
    #endregion
    #endregion

    #region Static Variables
    public static GameManager singleton;
    public static List<CropScript> crops;
    public static bool uiActive;
    public static bool dialogueActive;
    public static bool hasInputResponse;
    public List<Recipe> unlockedRecipes;

    public static QuestItem[] questItems => singleton._questItems;
    public static Transform Player => singleton.player;

    public static Transform recipeRequirementParent => singleton._recipeRequirementParent;
    public static GameObject recipeRequirementUI => singleton._recipeRequirementUI;

    #region Interact UI
    public static GameObject interactUI => singleton._interactUI;
    public static TextMeshProUGUI interactText => singleton._interactText;
    public static InteractableObject interactableObject => singleton._interactableObject;
    #endregion

    #region Shop UI
    public static GameObject shopUI => singleton._shopUI;
    public static GameObject cartUI => singleton._cartUI;
    public static GameObject shopDescriptionUI => singleton._shopDescriptionUI;
    public static TextMeshProUGUI shopNameText => singleton._shopNameText;
    public static TextMeshProUGUI shopCostText => singleton._shopCostText;
    public static TextMeshProUGUI shopDescriptionText => singleton._shopDescriptionText;
    public static TextMeshProUGUI shopAmountText => singleton._shopAmountText;
    public static TextMeshProUGUI shopIncrementText => singleton._shopIncrementText;
    public static TMP_InputField incrementInputField => singleton._incrementInputField;
    public static Button shopIncreaseButton => singleton._shopIncreaseButton;
    public static Button shopDecreaseButton => singleton._shopDecreaseButton;

    public static TextMeshProUGUI totalText => singleton._totalText;
    public static TextMeshProUGUI currentBalanceText => singleton._currentBalanceText;
    public static TextMeshProUGUI remainingBalanceText => singleton._remainingBalanceText;
    #endregion
    #endregion

    #region General Variables
    [Header("Important Player Variables")]
    public Transform player;
    public Transform playerCamera;
    public PlayerController playerController => player.GetComponent<PlayerController>();
    public int coins;
    #endregion

    #region UI Variables/Settings
    #region Recipe Book
    [Header("Recipe Book Settings")]
    [SerializeField] Image resultImage;
    [SerializeField] TextMeshProUGUI resultNameText;
    [SerializeField] TextMeshProUGUI resultAmountText;

    [Space()]
    [SerializeField] TextMeshProUGUI recipeNameText;
    [SerializeField] TextMeshProUGUI recipeDescriptionText;

    [Space()]
    public Transform _recipeUIParent;
    public GameObject _recipeUIPrefab;

    [Space()]
    public Transform _recipeRequirementParent;
    public GameObject _recipeRequirementUI;
    #endregion

    #region Menu UI
    [Header("Menu Settings")]
    public GameObject menuUI;
    #endregion

    #region Interact UI
    [Header("Interact UI Settings")]
    [SerializeField] GameObject _interactUI;
    [SerializeField] TextMeshProUGUI _interactText;
    #endregion

    #region Shop UI
    [Header("Shop UI Settings")]
    [SerializeField] GameObject _shopUI;
    [SerializeField] GameObject _cartUI;
    public Transform itemUIParent;
    public GameObject itemUI;
    public Transform cartItemUIParent;
    public GameObject cartItemUI;
    #endregion

    #region Shop Description UI
    [Header("Shop Description UI Settings")]
    [SerializeField] GameObject _shopDescriptionUI;
    [SerializeField] TextMeshProUGUI _shopNameText;
    [SerializeField] TextMeshProUGUI _shopCostText;
    [SerializeField] TextMeshProUGUI _shopDescriptionText;
    [SerializeField] TextMeshProUGUI _shopAmountText;
    [SerializeField] TextMeshProUGUI _shopIncrementText;
    [SerializeField] TMP_InputField _incrementInputField;
    [SerializeField] Button _shopIncreaseButton;
    [SerializeField] Button _shopDecreaseButton;
    #endregion

    #region Cart UI
    [Header("Cart UI Settings")]
    [SerializeField] TextMeshProUGUI _totalText;
    [SerializeField] TextMeshProUGUI _currentBalanceText;
    [SerializeField] TextMeshProUGUI _remainingBalanceText;
    #endregion
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public BuyStationScript selectedShop;
    [HideInInspector] public InteractableObject _interactableObject;
    [HideInInspector] public QuestItem[] _questItems;
    [HideInInspector] public List<RecipeItemUIScript> loadedRecipeItems;
    [HideInInspector] public List<GameObject> loadedShopItems;
    [HideInInspector] public List<GameObject> loadedCartItems;

    List<GameObject> loadedRecipeObjects;
    bool acceptedQuest;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        unlockedRecipes = new List<Recipe>();

        uiActive = false;
        dialogueActive = false;

        _interactableObject = new InteractableObject("", null);

        singleton = this;

        DisableInteractUI();
        DisableShopUI();

        crops = new List<CropScript>();
        _questItems = Resources.LoadAll<QuestItem>("Items/Quest Items");
        #endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            menuUI.SetActive(!menuUI.activeSelf);
            uiActive = menuUI.activeSelf;
        }

        #region Update Interact UI
        if (_interactableObject.gameObject == null || uiActive)
            DisableInteractUI();
        else
            UpdateInteractUI(_interactableObject.text);
        #endregion

        #region Update Shop UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableShopUI();
        }
        #endregion
    }
    #endregion

    #region Custom Functions
    #region Other Functions
    static void UnlockRecipe(Recipe recipe)
    {
        singleton.unlockedRecipes.Add(recipe);
    }
    #endregion

    #region Recipe Book Functions
    public void LoadRecipeResult(Recipe.RecipeItem result)
    {
        recipeNameText.text = result.item.name;
        recipeDescriptionText.text = result.item.description;

        resultImage.sprite = result.item.sprite;
        resultNameText.text = result.item.name;
        resultAmountText.text = $"x{result.amount}";
    }
    
    public void LoadRecipeBook()
    {
        foreach (GameObject loadedRecipeObject in loadedRecipeObjects)
            Destroy(loadedRecipeObject);

        loadedRecipeObjects.Clear();

        foreach (Recipe recipe in unlockedRecipes)
        {
            RecipeUIScript recipeUI = Instantiate(_recipeUIPrefab, _recipeUIParent.position, Quaternion.identity).GetComponent<RecipeUIScript>();
            recipeUI.LoadUI(recipe);

            loadedRecipeObjects.Add(recipeUI.gameObject);

            recipeUI.transform.SetParent(_recipeUIParent);
        }

        RectTransform rect = _recipeUIParent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, loadedRecipeObjects.Count * 200);
    }
    #endregion

    #region Helper Functions
    public static Outline AddOutline(GameObject obj, OutlineParams outlineParams)
    {
        Outline outline = obj.AddComponent<Outline>();

        #region Update Parameters
        outline.OutlineMode = outlineParams.outlineMode;
        outline.OutlineColor = outlineParams.color;
        outline.OutlineWidth = outlineParams.outlineWidth;
        #endregion

        return outline;
    }

    public static void ToggleCursor(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
        uiActive = active;
    }
    #endregion

    #region Other Helper Functions
    #region Shop Functions
    public void SubmitIncrementAmount(TMP_InputField inputField)
    {
        string stringInput = string.IsNullOrEmpty(inputField.text) ? "1" : inputField.text;
        int maxValue = Mathf.FloorToInt((coins / selectedShop.selectedItem.buyCost) - 
            (selectedShop.cart.ContainsKey(selectedShop.selectedItem) ? selectedShop.cart[selectedShop.selectedItem] : 0));
        int intInput = Mathf.Clamp(int.Parse(stringInput), 1, maxValue);

        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = intInput.ToString();
        inputField.text = intInput.ToString();
        
        selectedShop.SetIncrement(intInput);
    }

    public static void UpdateSelectedItem(Item item)
    {
        shopDescriptionUI.SetActive(true);
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
    #endregion

    #region Response Functions
    public static bool CheckIfInputtedResponse(Dialogue dialogue)
    {
        if (hasInputResponse)
            dialogue.acceptedQuest = singleton.acceptedQuest;
        return hasInputResponse;
    }

    public void ChangeResponseStatus(bool accepted)
    {
        acceptedQuest = accepted;
        hasInputResponse = true;
    }
    #endregion

    #region Interactable Object Functions
    public static void CheckIfInteractable(string interactText, GameObject gameObjectToCheck)
    {
        if (HoverScript.selectedGameObject == gameObjectToCheck && !isInteractableObject(gameObjectToCheck) && isEmpty())
            SetInteractableObject(interactText, gameObjectToCheck);
        else if (HoverScript.selectedGameObject != gameObjectToCheck && isInteractableObject(gameObjectToCheck))
            SetInteractableObject();
    }

    public static void SetInteractableObject(string text, GameObject selectedGameObject)
    {
        interactableObject.text = text;
        interactableObject.gameObject = selectedGameObject;
    }

    public static void SetInteractableObject()
    {
        interactableObject.text = "";
        interactableObject.gameObject = null;
    }

    public static bool isInteractableObject(GameObject gameObjectToCheck)
    {
        if (isEmpty()) return false; 
        return interactableObject.gameObject == gameObjectToCheck;
    }

    public static bool isEmpty()
    {
        return interactableObject.gameObject == null;
    }
    #endregion
    #endregion

    #region UI Functions
    #region Interact UI
    void UpdateInteractUI(string text)
    {
        _interactUI.SetActive(true);
        _interactText.text = "[E] " + text;
    }

    void DisableInteractUI()
    {
        _interactUI.SetActive(false);
    }
    #endregion

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
        ToggleCursor(false);
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
    #endregion
}