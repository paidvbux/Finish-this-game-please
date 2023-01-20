using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public static bool dialogueActive;

    public static Transform Player => singleton.player;

    //  Interactable Objects
    public static GameObject interactUI => singleton._interactUI;
    public static TextMeshProUGUI interactText => singleton._interactText;
    public static InteractableObject interactableObject => singleton._interactableObject;

    //  Dialogue UI
    public static GameObject DialogueUI => singleton._dialogueUI;
    public static TextMeshProUGUI DialogueName => singleton._dialogueName;
    public static TextMeshProUGUI DialogueText => singleton._dialogueText;
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
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public InteractableObject _interactableObject;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        //  Initialize classes.
        _interactableObject = new InteractableObject("", null);

        //  Start with the interact UI disabled.
        DisableInteractUI();
        //  Sets the singleton to this.
        singleton = this;

        //  Initialize lists
        crops = new List<CropScript>();
        #endregion
    }

    void Update()
    {
        #region Update Interact UI
        //  Update the interact UI with the text.
        if (_interactableObject.gameObject == null)
            DisableInteractUI();
        else
            UpdateInteractUI(_interactableObject.text);
        #endregion       
    }
    #endregion

    #region Custom Functions
    /*
     *   Adds an outline to the specified object.
     *   The outlines settings can be tweaked from
     *   when it is called.
     */
    public static Outline AddOutline(GameObject obj, OutlineParams outlineParams)
    {
        //  Adds the component.
        Outline outline = obj.AddComponent<Outline>();

        #region Update Parameters
        //  Changes the settings of the outline.
        outline.OutlineMode = outlineParams.outlineMode;
        outline.OutlineColor = outlineParams.color;
        outline.OutlineWidth = outlineParams.outlineWidth;
        #endregion

        //  Returns the component.
        return outline;
    }

    #region Interactable Object Functions
    //  Sets the object to the selected object.
    public static void SetInteractableObject(string text, GameObject gameObjectToAdd)
    {
        interactableObject.text = text;
        interactableObject.gameObject = gameObjectToAdd;
    }

    //  Empty the interactableObject variable.
    public static void SetInteractableObject()
    {
        interactableObject.text = "";
        interactableObject.gameObject = null;
    }

    //  Returns if the current value is the same as the given value.
    public static bool isInteractableObject(GameObject gameObjectToCheck)
    {
        if (isEmpty()) return false; 
        return interactableObject.gameObject == gameObjectToCheck;
    }

    //  Returns if the value is empty.
    public static bool isEmpty()
    {
        return interactableObject.gameObject == null;
    }
    #endregion

    #region UI Functions
    #region Interact UI
    /*
     *   Toggles the UI that shows how to interact
     *   with objects. Changes text from input.
     */
    void UpdateInteractUI(string text)
    {
        interactUI.SetActive(true);
        _interactText.text = "[E] " + text;
    }
    void DisableInteractUI()
    {
        interactUI.SetActive(false);
    }
    #endregion

    #region Dialogue UI
   /*
    *   Toggles the active state
    *   of the dialogue UI.
    */
    public static void ToggleDialogueUI(bool dialogueActive, string speakerName = "")
    {
        DialogueUI.SetActive(dialogueActive);
        DialogueName.text = dialogueActive ? speakerName : "";
    }

   /*
    *   Updates the 
    *   dialogue's text.
    */
    public static void UpdateDialogueUI(string text)
    {
        DialogueText.text = text;
    }
    #endregion
    #endregion
    #endregion
}