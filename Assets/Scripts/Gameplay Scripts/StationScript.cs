using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationScript : MonoBehaviour
{
    #region Other Variables
    enum StationType { Sell, Buy }
    [SerializeField] StationType stationType;
    #endregion

    #region Trigger Areas
    [Header("Trigger Areas")]
    public TriggerScript playerInteractTrigger;
    public TriggerScript inputTrigger;
    #endregion

    #region Sell Station Variables/Settings
    [HideInInspector] public List<CrateScript> crates;
    [HideInInspector] public List<DroppedItemScript> droppedItems;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        //  Checks if the player is trying to interact with the station.
        if (playerInteractTrigger.inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            //  Determine what type of operation the player is doing.
            if (stationType == StationType.Sell) 
                Sell();
        }
    }
    #endregion

    #region Custom Functions
   /*
    *   Sorts the items in the station 
    *   into crates and items.
    */
    void SortItems()
    {
        //  Reset the lists.
        crates.Clear();
        droppedItems.Clear();

        //  Go through each item and sort them depending on their value.
        foreach (KeyValuePair<GameObject, string> storedObject in inputTrigger.storedObjects)
        {
            if (storedObject.Value == "Crate")
                crates.Add(storedObject.Key.GetComponent<CrateScript>());
            if (storedObject.Value == "Dropped Item")
                droppedItems.Add(storedObject.Key.GetComponent<DroppedItemScript>());
        }
    }

   /*
    *   Adds money to the balance of the player 
    *   depending on the price of each item and
    *   the amount.
    */
    void Sell()
    {
        //  Sorts the items.
        SortItems();

        //  Creates a list for future use.
        List<DroppedItemScript> droppedItemsToRemove = new List<DroppedItemScript>();

        //  Go through each crate and sell each item. Does not delete the box but empties the contents.
        foreach (CrateScript crate in crates)
        {
            if (crate.storedItem != null)
            {
                GameManager.singleton.coins += crate.storedItem.sellCost * crate.storedAmount;
                crate.EmptyCrate(false);
            }
        }

        //  Adds the item to the droppedItemsToRemove list. Adds the amount of money depending on their cost.
        foreach (DroppedItemScript droppedItem in droppedItems)
        {
            GameManager.singleton.coins += droppedItem.item.sellCost;
            droppedItemsToRemove.Add(droppedItem);
        }

        //Remove and destroy the items after selling them.
        foreach (DroppedItemScript droppedItem in droppedItemsToRemove)
        {
            Destroy(droppedItem.gameObject);
            inputTrigger.storedObjects.Remove(droppedItem.gameObject);
            droppedItems.Remove(droppedItem);
        }
    }
    #endregion
}
