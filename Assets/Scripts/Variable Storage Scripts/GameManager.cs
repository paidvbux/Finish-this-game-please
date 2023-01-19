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
    public static Transform Player => GameManager.singleton.player;
    public static List<CropScript> crops;
    #endregion

    #region General Variables
    [Header("Important Player Variables")]
    public Transform player;
    public PlayerController playerController => player.GetComponent<PlayerController>();
    public int coins;

    [Header("UI Settings")]
    [SerializeField] GameObject interactUI;
    [SerializeField] TextMeshProUGUI interactText;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public List<InteractableObject> interactableObjects;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        //  Start with the interact UI disabled.
        DisableInteractUI();
        //  Sets the singleton to this.
        singleton = this;

        //  Initialize lists
        interactableObjects = new List<InteractableObject>();
        crops = new List<CropScript>();
        #endregion
    }

    void Update()
    {
        #region Update Interact UI
        //  Update the interact UI with the text.
        if (interactableObjects.Count == 0)
            DisableInteractUI();
        else
            UpdateInteractUI(interactableObjects[0].text);
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

    public void AddToInteractableObjects(string text, GameObject gameObjectToAdd)
    {
        //  Adds the object to the list.
        interactableObjects.Add(new InteractableObject(text, gameObjectToAdd));
    }

    public void RemoveFromInteractableObjects(GameObject gameObjectToRemove)
    {
        InteractableObject objectToRemove = null;

        //  Find the object to remove from the list and remove it.
        foreach (InteractableObject interactableObject in interactableObjects)
        {
            if (interactableObject.gameObject == gameObjectToRemove)
                objectToRemove = interactableObject;
        }

        //  Removes it from the list.
        interactableObjects.Remove(objectToRemove);
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