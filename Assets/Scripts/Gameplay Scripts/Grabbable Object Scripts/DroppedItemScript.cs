using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : GrabbableObjectScript
{
    #region Item Variables/Settings
    [Header("Item Settings")]
    public Item item;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public List<TriggerScript> triggers;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Start()
    {
        #region Initialization
        //  Sets the mass of the object
        rigidBody.mass = item.mass;

        //  Sets the tag to "Dropped Item" to prevent bugs
        gameObject.tag = "Dropped Item";

        //  Runs the derived initialization
        Initialize();
        #endregion
    }

    void Update()
    {
        CheckForHighlight();
    }

    void OnTriggerEnter(Collider other)
    {
        AddToCrate(other);
    }

    void OnDestroy()
    {
        if (triggers.Count > 0)
        {
            foreach (TriggerScript trigger in triggers)
            {
                trigger.storedObjects.Remove(gameObject);
            }
        }
    }
    #endregion

    #region Custom Functions
    public void AddToCrate(Collider other)
    {
        if (other.gameObject.CompareTag("Crate") && other.gameObject.TryGetComponent<CrateScript>(out CrateScript crateScript) && !crateScript.isFull())
        {
            #region Add to Crate
            crateScript.AddItem(item);
            HoverScript.selectedGameObject = null;
            GrabScript.holdingObject = false;
            Destroy(gameObject);
            #endregion 
        }
    }
    #endregion
}
