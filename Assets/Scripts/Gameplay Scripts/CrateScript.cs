using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : GrabbableObjectScript
{
    [Header("Storage Settings")]
    public Item storedItem;
    public int storedAmount;

    [Header("Display Settings")]
    public Texture2D storedTexture;

    float startingMass;

    void Awake()
    {
        gameObject.tag = "Crate";
        startingMass = rigidBody.mass;
        Initialize();
    }

    void Update()
    {
        if (storedItem != null && storedAmount > 0 && HoverScript.selectedGameObject == gameObject)
        {
            EmptyCrate(true);
        }
        CheckForHighlight();
    }

    void UpdateTexture()
    {
    }

    public void EmptyCrate(bool giveBackItems)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (giveBackItems)
            {
                for (int i = 0; i < storedAmount; i++)
                {
                    GameObject crop = Instantiate(storedItem.grabbableCropObject, transform.position + transform.forward * 0.1f, Quaternion.identity);
                    crop.transform.SetParent(GameManager.singleton.transform);
                }
            }
            storedItem = null;
            storedAmount = 0;
        }
    }

    public bool isFull()
    {
        if (storedItem == null)
            return false;
        return storedAmount >= storedItem.maxCrateStorage;
    }

    public void AddItem(Item item)
    {
        if (storedItem == null)
        {
            storedItem = item;
            storedAmount = 0;
        }

        rigidBody.mass += storedItem.mass;
        storedAmount++;
    }
}
