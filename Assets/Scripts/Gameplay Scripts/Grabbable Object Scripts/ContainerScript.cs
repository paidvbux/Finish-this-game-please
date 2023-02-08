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
        public bool isSeedPacket;

        public StoredItem(Item _item, int _count, bool _isSeedPacket)
        {
            item = _item;
            count = _count;
            isSeedPacket = _isSeedPacket;
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
            if (item.isSeedPacket)
                CreateSeedPacket(item, spawnPosition);
            else
                CreateItems(item, spawnPosition);
        }

        Destroy(gameObject);
    }

    void CreateSeedPacket(StoredItem item, Vector3 spawnPosition)
    {
        SeedPacketScript seedPacket = Instantiate(item.item.grabbableObject, spawnPosition, Quaternion.identity).GetComponent<SeedPacketScript>();
        seedPacket.transform.SetParent(GameManager.singleton.transform);
        seedPacket.seedsLeft = item.count;
    }

    void CreateItems(StoredItem item, Vector3 spawnPosition)
    {
        for (int i = 0; i < item.count; i++)
        {
            GameObject itemObject = Instantiate(item.item.grabbableObject, spawnPosition, Quaternion.identity);
            itemObject.transform.SetParent(GameManager.singleton.transform);
        }
    }
    #endregion
}
