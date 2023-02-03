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

    public static Item[] items => singleton._items;

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

    #region Shop Description
    public static GameObject shopUI => singleton._shopUI;
    public static TextMeshProUGUI shopNameText => singleton._shopNameText;
    public static TextMeshProUGUI shopCostText => singleton._shopCostText;
    public static TextMeshProUGUI shopDescriptionText => singleton._shopDescriptionText;
    public static TextMeshProUGUI shopAmountText => singleton._shopAmountText;
    public static Button shopIncreaseButton => singleton._shopIncreaseButton;
    public static Button shopDecreaseButton => singleton._shopDecreaseButton;
    #endregion
    #endregion

    #region General Variables
    [Header("Important Player Variables")]
    public Transform player;
    public PlayerController playerController => player.GetComponent<PlayerController>();
    public int coins;
    #endregion

    #region UI Variables/Settings
    [Header("UI Settings")]
    [SerializeField] GameObject _interactUI;
    [SerializeField] TextMeshProUGUI _interactText;

    [Space()]
    [SerializeField] GameObject _dialogueUI;
    [SerializeField] TextMeshProUGUI _dialogueName;
    [SerializeField] TextMeshProUGUI _dialogueText;

    [Space()]
    [SerializeField] GameObject _dialogueChoices;

    [Space()]
    [SerializeField] GameObject _shopUI;
    [SerializeField] TextMeshProUGUI _shopNameText;
    [SerializeField] TextMeshProUGUI _shopCostText;
    [SerializeField] TextMeshProUGUI _shopDescriptionText;
    [SerializeField] TextMeshProUGUI _shopAmountText;
    [SerializeField] Button _shopIncreaseButton;
    [SerializeField] Button _shopDecreaseButton;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public InteractableObject _interactableObject;
    [HideInInspector] public Item[] _items;
    bool acceptedQuest;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        _interactableObject = new InteractableObject("", null);

        DisableInteractUI();
        singleton = this;

        crops = new List<CropScript>();
        _items = Resources.LoadAll<Item>("Items");

        _dialogueUI.SetActive(false);
        #endregion
    }

    void Update()
    {
        #region Update Interact UI
        if (_interactableObject.gameObject == null)
            DisableInteractUI();
        else
            UpdateInteractUI(_interactableObject.text);
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
    
    public static void CloseShopUI()
    {
        shopUI.SetActive(false);
        ToggleCursor(false);
    }

    public static void UpdateShopDescriptionUI(Item item, int amount)
    {
        shopUI.SetActive(true);
        shopNameText.text = item.name;
        shopCostText.text = item.buyCost.ToString();
        shopDescriptionText.text = item.description;
        shopAmountText.text = amount.ToString();
    }

    public static void ToggleCursor(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
        uiActive = active;
    }

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

    public static void SetInteractableObject(string text, GameObject gameObjectToAdd)
    {
        interactableObject.text = text;
        interactableObject.gameObject = gameObjectToAdd;
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
    #endregion
    #endregion
}