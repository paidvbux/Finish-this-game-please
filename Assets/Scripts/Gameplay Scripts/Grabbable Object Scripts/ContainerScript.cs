using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerScript : GrabbableObjectScript
{
    #region Classes
    [System.Serializable]
    public class StoredItem
    {
        public Item item;
        public int count;

        public StoredItem(Item _item, int _count)
        {
            item = _item;
            count = _count;
        }
    }
    #endregion

    #region Container Variables/Settings
    [Header("Container Settings")]
    public List<StoredItem> storedItems = new List<StoredItem>();
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        GameManager.CheckIfInteractable("Empty", gameObject);
        
        if (GameManager.isInteractableObject(gameObject) && Input.GetKeyDown(KeyCode.E))
        {
            EmptyContainer();
        }

        CheckForHighlight();
    }
    #endregion

    #region Custom Functions
    void EmptyContainer()
    {
        Vector3 spawnPosition = ((GameManager.Player.transform.position - transform.position).normalized + Vector3.up) / 10f + transform.position;
        foreach (StoredItem item in storedItems)
        {
            for (int i = 0; i < item.count; i++)
            {
                GameObject itemObject = Instantiate(item.item.grabbableObject, spawnPosition, Quaternion.identity);
                itemObject.transform.SetParent(GameManager.singleton.transform);
            }
        }

        Destroy(gameObject);
    }
    #endregion
}
