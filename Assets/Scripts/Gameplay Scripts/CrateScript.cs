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

        //  Sets its tag to "Crate" to allow for the GrabScript to properly interact with it.
        gameObject.tag = "Crate";
        startingMass = rigidBody.mass;

        //  Runs the derived initialization.
        Initialize();
    }

    void Update()
    {
        //  Checks if the crate can be emptied or not
        if (storedItem != null && storedAmount > 0 && HoverScript.selectedGameObject == gameObject)
        {
            EmptyCrate(true);
        }

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

            //  Resets the mass of the crate because there is nothing inside anymore.
            rigidBody.mass = startingMass;

            //  Resets some values to prevent bugs.
            storedItem = null;
            storedAmount = 0;
            
            //  Resets the crate's display to be empty.
            uiInfo.quadDisplay.UpdateTexture("Other/Empty");
        }
    }

   /*
    *   Adds an item to the crate; increases the count
    *   by one.
    */
    public void AddItem(Item item)
    {
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
            nameInfo.text.text = storedItem.name;
            storedAmount = 0;
        }

        //  Increases the mass and the amount stored inside.
        rigidBody.mass += storedItem.mass;
        storedAmount++;

        //  Updates the number of items stored.
        countInfo.text.text = storedAmount + " / " + storedItem.maxCrateStorage;
        uiInfo.quadDisplay.UpdateTexture(storedItem.name + "/" + storedItem.name + " " + storedAmount.ToString());
    }

   /*
    *   Returns if the crate is full or not
    */
    public bool isFull()
    {
        //  Exit function early if the crate is empty.
        if (storedItem == null)
            return false;
        return storedAmount >= storedItem.maxCrateStorage;
    }
    #endregion
}
