using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : GrabbableObjectScript
{
    #region Storage Variables/Settings
    [Header("Storage Settings")]
    public Item storedItem;
    public int storedAmount;
    #endregion

    #region Display Variables/Settings
    [Header("Display Settings")]
    public UpdateUIInfo uiInfo;
    UpdateUIInfo.TMPInfo nameInfo;
    UpdateUIInfo.TMPInfo countInfo;
    UpdateUIInfo.ImageInfo mainImageInfo;
    #endregion

    #region Private/Hidden Variables
    float startingMass;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Map UI Info
        foreach (UpdateUIInfo.ImageInfo imageInfo in uiInfo.imagesToUpdate)
        {
            if (imageInfo.referenceName == "Main Image")
            {
                mainImageInfo = imageInfo;
                continue;
            }
        }

        foreach (UpdateUIInfo.TMPInfo textInfo in uiInfo.textToUpdate)
        {
            if (textInfo.name == "Name")
            {
                nameInfo = textInfo;
                continue;
            }
            if (textInfo.name == "Count")
            {
                countInfo = textInfo;
                continue;
            }
        }
        #endregion

        #region Initialization
        gameObject.tag = "Crate";
        startingMass = rigidBody.mass;

        Initialize();
        #endregion
    }

    void Update()
    {
        #region Empty Contents
        if (storedItem != null && storedAmount > 0 && HoverScript.selectedGameObject == gameObject && !GameManager.isInteractableObject(gameObject) && GameManager.isEmpty())
            GameManager.SetInteractableObject("Empty Crate", gameObject);
        else if ((storedItem == null || storedAmount == 0 || HoverScript.selectedGameObject != gameObject) && GameManager.isInteractableObject(gameObject))
            GameManager.SetInteractableObject();

        if (GameManager.isInteractableObject(gameObject))
            EmptyCrate(true);
        #endregion

        CheckForHighlight();
    }
    #endregion

    #region Custom Functions
    public void EmptyCrate(bool giveBackItems)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            #region Return Items
            if (giveBackItems) 
            {
                for (int i = 0; i < storedAmount; i++)
                {
                    GameObject crop = Instantiate(storedItem.grabbableObject, 
                        transform.position + (GameManager.Player.position - transform.position) * 0.1f, Quaternion.identity);
                    crop.transform.SetParent(GameManager.singleton.transform);
                }
            }
            #endregion

            #region Reset Variables
            rigidBody.mass = startingMass;

            storedItem = null;
            storedAmount = 0;

            nameInfo.ChangeValue(Color.white, "Empty");
            countInfo.ChangeValue(Color.white, "- / -");
            uiInfo.UpdateUIElements();
            uiInfo.quadDisplay.UpdateTexture("Other/Empty");
            #endregion
        }
    }

    public void AddItem(Item item)
    {
        #region Reset Empty
        if (storedItem == null)
        {
            storedItem = item;
            nameInfo.ChangeValue(nameInfo.color, storedItem.itemName);
            if (item.sprite != null)
                mainImageInfo.ChangeValue(item.sprite, Color.white);
            else
                mainImageInfo.ChangeValue(null, new Color(0, 0, 0, 0));
            
            storedAmount = 0;
        }
        #endregion

        #region Update Values/UI
        rigidBody.mass += storedItem.mass;
        storedAmount++;

        countInfo.ChangeValue(isFull() ? Color.red : Color.white, storedAmount + " / " + storedItem.maxCrateStorage);
        uiInfo.UpdateUIElements();
        uiInfo.quadDisplay.UpdateTexture(storedItem.itemName + "/" + storedItem.itemName + " " + storedAmount.ToString());
        #endregion
    }

    public bool isFull()
    {
        #region Returns Value
        if (storedItem == null)
            return false;
        return storedAmount >= storedItem.maxCrateStorage;
        #endregion
    }
    #endregion
}
