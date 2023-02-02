using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellStationScript : StationScript
{
    #region Trigger Areas
    [Header("Trigger Areas")]
    public TriggerScript inputTrigger;
    #endregion

    #region Sell Station Variables/Settings
    [HideInInspector] public List<CrateScript> crates;
    [HideInInspector] public List<DroppedItemScript> droppedItems;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    void SortItems()
    {
        #region Clear Lists
        crates.Clear();
        droppedItems.Clear();
        #endregion

        #region Sort Objects
        foreach (KeyValuePair<GameObject, string> storedObject in inputTrigger.storedObjects)
        {
            if (storedObject.Value == "Crate")
                crates.Add(storedObject.Key.GetComponent<CrateScript>());
            if (storedObject.Value == "Dropped Item")
                droppedItems.Add(storedObject.Key.GetComponent<DroppedItemScript>());
        }
        #endregion
    }

    public override void Interact()
    {
        SortItems();

        List<DroppedItemScript> droppedItemsToRemove = new List<DroppedItemScript>();

        #region Sell Crates
        foreach (CrateScript crate in crates)
        {
            if (crate.storedItem != null)
            {
                GameManager.singleton.coins += crate.storedItem.sellCost * crate.storedAmount;
                crate.EmptyCrate(false);
            }
        }
        #endregion

        #region Sell Items
        foreach (DroppedItemScript droppedItem in droppedItems)
        {
            GameManager.singleton.coins += droppedItem.item.sellCost;
            droppedItemsToRemove.Add(droppedItem);
        }
        #endregion

        #region Destroy Dropped Items
        foreach (DroppedItemScript droppedItem in droppedItemsToRemove)
        {
            Destroy(droppedItem.gameObject);
            inputTrigger.storedObjects.Remove(droppedItem.gameObject);
            droppedItems.Remove(droppedItem);
        }
        #endregion
    }
    #endregion
}
