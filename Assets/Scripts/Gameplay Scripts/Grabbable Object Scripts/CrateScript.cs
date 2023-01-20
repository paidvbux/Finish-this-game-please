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
    #endregion

    #region Private/Hidden Variables
    float startingMass;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Map UI Info
        //  Sets the nameInfo and countInfo to be a global variable for easier access.
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
        //  Sets its tag to "Crate" to allow for the GrabScript to properly interact with it.
        gameObject.tag = "Crate";
        startingMass = rigidBody.mass;

        //  Runs the derived initialization.
        Initialize();
        #endregion
    }

    void Update()
    {
        #region Empty Contents

        //  Checks if the crate can be emptied or not
        if (storedItem != null && storedAmount > 0 && HoverScript.selectedGameObject == gameObject && !GameManager.isInteractableObject(gameObject) && GameManager.isEmpty())
        {
            //  Sets the UI to display this gameObject.
            GameManager.SetInteractableObject("Empty Crate", gameObject);
        }
        else if ((storedItem == null || storedAmount == 0 || HoverScript.selectedGameObject != gameObject) && GameManager.isInteractableObject(gameObject))
        {
            //  Removes the UI to display.
            GameManager.SetInteractableObject();
        }

        if (GameManager.isInteractableObject(gameObject))
            EmptyCrate(true);
        #endregion

        //  Runs the highlight function from its derived class
        CheckForHighlight();
    }
    #endregion

    #region Custom Functions
   /*
    *   Empties the crate so that the player can replace the 
    *   contents of the crate or to sell the contents of the
    *   crate.
    */
    public void EmptyCrate(bool giveBackItems)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            #region Return Items
            /* 
             *   Determines whether or not to return the dropped items back to the player.
             *   true  : Used when emptying the crate.
             *   false : Used when selling contents of the crates.
             */
            if (giveBackItems) 
            {
                for (int i = 0; i < storedAmount; i++)
                {
                    //  Duplicates the item towards the player to prevent it from clipping through objects.
                    GameObject crop = Instantiate(storedItem.grabbableCropObject, 
                        transform.position + (GameManager.Player.position - transform.position) * 0.1f, Quaternion.identity);
                    crop.transform.SetParent(GameManager.singleton.transform);
                }
            }
            #endregion

            #region Reset Variables
            //  Resets the mass of the crate because there is nothing inside anymore.
            rigidBody.mass = startingMass;

            //  Resets some values to prevent bugs.
            storedItem = null;
            storedAmount = 0;

            //  Resets the crate's display to be empty.
            nameInfo.ChangeValue(Color.white, "Empty");
            countInfo.ChangeValue(Color.white, "- / -");
            uiInfo.UpdateUIElements();
            uiInfo.quadDisplay.UpdateTexture("Other/Empty");
            #endregion
        }
    }

   /*
    *   Adds an item to the crate; increases the count
    *   by one.
    */
    public void AddItem(Item item)
    {
        #region Reset Empty
        /*
         *   Checks if the crate is empty and changes values
         *   depending on if it is.
         * 
         *   Empty?
         *       Yes : Sets the item to the one added and 
         *             updates the name on the display.
         *       No  : Skips the step.
         */

        if (storedItem == null)
        {
            storedItem = item;
            nameInfo.ChangeValue(nameInfo.color, storedItem.itemName);
            storedAmount = 0;
        }
        #endregion

        #region Update Values/UI
        //  Increases the mass and the amount stored inside.
        rigidBody.mass += storedItem.mass;
        storedAmount++;

        //  Updates the number of items stored.
        countInfo.ChangeValue(isFull() ? Color.red : Color.white, storedAmount + " / " + storedItem.maxCrateStorage);
        uiInfo.UpdateUIElements();
        uiInfo.quadDisplay.UpdateTexture(storedItem.itemName + "/" + storedItem.itemName + " " + storedAmount.ToString());
        #endregion
    }

   /*
    *   Returns if the crate is full or not
    */
    public bool isFull()
    {
        #region Returns Value
        //  Exit function early if the crate is empty.
        if (storedItem == null)
            return false;
        return storedAmount >= storedItem.maxCrateStorage;
        #endregion
    }
    #endregion
}
