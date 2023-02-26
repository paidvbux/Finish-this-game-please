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

    public static QuestItem[] questItems => singleton._questItems;
    public static Transform Player => singleton.player;

    #region Interact UI
    public static GameObject interactUI => singleton._interactUI;
    public static TextMeshProUGUI interactText => singleton._interactText;
    public static InteractableObject interactableObject => singleton._interactableObject;
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
    #region Menu UI
    [Header("Menu Settings")]
    public GameObject menuUI;
    #endregion

    #region Interact UI
    [Header("Interact UI Settings")]
    [SerializeField] GameObject _interactUI;
    [SerializeField] TextMeshProUGUI _interactText;
    #endregion
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
        uiActive = false;
        dialogueActive = false;

        _interactableObject = new InteractableObject("", null);

        singleton = this;

        DisableInteractUI();

        crops = new List<CropScript>();
        _questItems = Resources.LoadAll<QuestItem>("Items/Quest Items");
        #endregion
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
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
    }
    #endregion

    #region Custom Functions
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
    #endregion
    #endregion
}