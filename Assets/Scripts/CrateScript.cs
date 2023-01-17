using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : GrabbableObjectScript
{
    [Header("Storage Settings")]
    public Crop storedCrop;
    public int storedAmount;

    float startingMass;

    void Awake()
    {
        gameObject.tag = "Crate";
        startingMass = rigidBody.mass;
        Initialize();
    }

    void Update()
    {
        if (storedCrop != null && storedAmount > 0 && HoverScript.selectedGameObject == gameObject)
        {
            EmptyCrate();
        }
        CheckForHighlight();
    }

    void EmptyCrate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < storedAmount; i++)
            {
                GameObject crop = Instantiate(storedCrop.grabbableCropObject, transform.position + transform.forward * 0.1f, Quaternion.identity);
                crop.transform.SetParent(GameManager.singleton.transform);
            }
            storedCrop = null;
            storedAmount = 0;
        }
    }

    public bool isFull()
    {
        if (storedCrop == null)
            return false;
        return storedAmount >= storedCrop.maxCrateStorage;
    }

    public void AddItem(Crop crop)
    {
        if (storedCrop == null)
        {
            storedCrop = crop;
            storedAmount = 0;
        }

        rigidBody.mass += storedCrop.mass;
        storedAmount++;
    }
}
