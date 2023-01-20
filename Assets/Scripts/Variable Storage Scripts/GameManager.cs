using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Classes
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

    #region Static Variables
    public static GameManager singleton;
    public static List<CropScript> crops;
    public static bool dialogueActive;

    public static Transform Player => singleton.player;

    public static TextMeshProUGUI DialogueName => singleton.dialogueName;
    public static TextMeshProUGUI DialogueText => singleton.dialogueText;
    #endregion

    #region General Variables
    [Header("Important Player Variables")]
    public Transform player;
    public PlayerController playerController => player.GetComponent<PlayerController>();
    public int coins;

    [Header("UI Settings")]
    [SerializeField] GameObject interactUI;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI dialogueName;
    [SerializeField] TextMeshProUGUI dialogueText;
    #endregion

    #region Hidden/Private Variables
    InteractableObject interactableObject;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        //  Initialize classes.
        interactableObject = new InteractableObject("", null);

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
        if (interactableObject.gameObject == null)
            DisableInteractUI();
        else
            UpdateInteractUI(interactableObject.text);
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

    //  Sets the object to the selected object.
    public void SetInteractableObject(string text, GameObject gameObjectToAdd)
    {
        interactableObject = new InteractableObject(text, gameObjectToAdd);
    }

    //  Empty the interactableObject variable.
    public void SetInteractableObject()
    {
        interactableObject.text = "";
        interactableObject.gameObject = null;
    }

    //  Returns if the current value is the same as the given value.
    public bool isInteractableObject(GameObject gameObjectToCheck)
    {
        if (isEmpty()) return false; 
        return interactableObject.gameObject == gameObjectToCheck;
    }

    //  Returns if the value is empty.
    public bool isEmpty()
    {
        return interactableObject.gameObject == null;
    }

   /*
    *   Toggles the UI that shows how to interact
    *   with objects. Changes text from input.
    */
    void UpdateInteractUI(string text)
    {
        interactUI.SetActive(true);
        interactText.text = "[E] " + text;
    }
    void DisableInteractUI()
    {
        interactUI.SetActive(false);
    }
    #endregion
}