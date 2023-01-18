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

    void Start()
    {

    }

    void Update()
    {
        if (playerInteractTrigger.inTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (stationType == StationType.Sell) 
                Sell();
        }
    }

    void SortItems()
    {
        crates.Clear();
        droppedItems.Clear();
        foreach (KeyValuePair<GameObject, string> storedObject in inputTrigger.storedObjects)
        {
            if (storedObject.Value == "Crate")
                crates.Add(storedObject.Key.GetComponent<CrateScript>());
            if (storedObject.Value == "Dropped Item")
                droppedItems.Add(storedObject.Key.GetComponent<DroppedItemScript>());
        }
    }

    void Sell()
    {
        SortItems();

        List<DroppedItemScript> droppedItemsToRemove = new List<DroppedItemScript>();

        //Selling the items and destroying the objects
        foreach (CrateScript crate in crates)
        {
            if (crate.storedItem != null)
            {
                GameManager.singleton.coins += crate.storedItem.sellCost * crate.storedAmount;
                crate.EmptyCrate(false);
            }
        }

        foreach (DroppedItemScript droppedItem in droppedItems)
        {
            GameManager.singleton.coins += droppedItem.item.sellCost;
            droppedItemsToRemove.Add(droppedItem);
        }

        //Remove the items after destroying them
        foreach (DroppedItemScript droppedItem in droppedItemsToRemove)
        {
            Destroy(droppedItem.gameObject);
            inputTrigger.storedObjects.Remove(droppedItem.gameObject);
            droppedItems.Remove(droppedItem);
        }
    }
}
