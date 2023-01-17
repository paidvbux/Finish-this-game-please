using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationScript : MonoBehaviour
{
    enum StationType { Sell, Buy }
    [SerializeField] StationType stationType;

    [Header("Trigger Areas")]
    public TriggerScript playerInteractTrigger;
    public TriggerScript inputTrigger;

    [Header("Selling Station Settings")]
    public List<CrateScript> crates;
    public List<DroppedItemScript> droppedItems;

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
        foreach (CrateScript crate in crates)
        {
            GameManager.singleton.coins += crate.storedItem.sellCost * crate.storedAmount;
            crate.EmptyCrate(false);
        }

        foreach (DroppedItemScript droppedItem in droppedItems)
        {
            GameManager.singleton.coins += droppedItem.item.sellCost;
            Destroy(droppedItem.gameObject);
        }
    }
}
