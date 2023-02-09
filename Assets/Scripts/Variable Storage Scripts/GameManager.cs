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
    public static bool hasInputResponse;

    public static QuestItem[] questItems => singleton._questItems;

    public static Transform Player => singleton.player;

    #region Interact UI
    public static GameObject interactUI => singleton._interactUI;
    public static TextMeshProUGUI interactText => singleton._interactText;
    public static InteractableObject interactableObject => singleton._interactableObject;
    #endregion

    #region Dialogue UI
    public static GameObject DialogueUI => singleton._dialogueUI;
    public static GameObject DialogueChoices => singleton._dialogueChoices;
    public static TextMeshProUGUI DialogueName => singleton._dialogueName;
    public static TextMeshProUGUI DialogueText => singleton._dialogueText;
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
    public PlayerController playerController => player.GetComponent<PlayerController>();
    public int coins;

    public BuyStationScript selectedShop;
    #endregion

    #region UI Variables/Settings
    [Header("Interact UI Settings")]
    [SerializeField] GameObject _interactUI;
    [SerializeField] TextMeshProUGUI _interactText;

    [Header("Dialogue UI Settings")]
    [SerializeField] GameObject _dialogueUI;
    [SerializeField] TextMeshProUGUI _dialogueName;
    [SerializeField] TextMeshProUGUI _dialogueText;
    
    [Space()]
    [SerializeField] GameObject _dialogueChoices;

    [Header("Shop UI Settings")]
    [SerializeField] GameObject _shopUI;
    [SerializeField] GameObject _cartUI;
    public Transform itemUIParent;
    public GameObject itemUI;
    public Transform cartItemUIParent;
    public GameObject cartItemUI;

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

    [Header("Cart UI Settings")]
    [SerializeField] TextMeshProUGUI _totalText;
    [SerializeField] TextMeshProUGUI _currentBalanceText;
    [SerializeField] TextMeshProUGUI _remainingBalanceText;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public InteractableObject _interactableObject;
    [HideInInspector] public QuestItem[] _questItems;
    bool acceptedQuest;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        _interactableObject = new InteractableObject("", null);

        singleton = this;

        DisableInteractUI();
        DisableShopUI();

        crops = new List<CropScript>();
        _questItems = Resources.LoadAll<QuestItem>("Items/Quest Items");

        _dialogueUI.SetActive(false);
        #endregion
    }

    void Update()
    {
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

    #region Dialogue UI
    public static void ToggleDialogueUI(bool active, string speakerName = "")
    {
        DialogueUI.SetActive(active);
        DialogueName.text = active ? speakerName : "";
        ToggleCursor(active);
    }

    public static void ToggleDialogueChoices(bool active)
    {
        DialogueChoices.SetActive(active);
    }

    public static void UpdateDialogueUI(string text)
    {
        DialogueText.text = text;
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