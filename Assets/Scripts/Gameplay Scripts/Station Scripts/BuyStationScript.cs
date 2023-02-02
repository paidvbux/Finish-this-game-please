using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyStationScript : StationScript
{
    #region Classes
    [System.Serializable]
    public class PurchasableItem
    {
        public string name;
        public Item item;
        public int cost;
    }
    #endregion

    #region Trigger Areas
    [Header("Trigger Areas")]
    public TriggerScript playerTrigger;
    #endregion

    #region Buy Station Variables/Settings
    public Dictionary<PurchasableItem, int> cart;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public override void Interact()
    {

    }

    public void AddToCart(PurchasableItem item, int amountToAdd)
    {
        if (cart.ContainsKey(item) && item.cost * (cart[item] + 1) <= GameManager.singleton.coins)
            cart[item] += amountToAdd;
        else if (!cart.ContainsKey(item))
            cart.Add(item, amountToAdd);        
    }
    #endregion
}
