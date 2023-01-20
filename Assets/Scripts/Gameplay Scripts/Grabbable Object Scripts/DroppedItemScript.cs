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
        //  Runs the highlight function from its derived class
        CheckForHighlight();
    }

    void OnTriggerEnter(Collider other)
    {
       /*
        *  Checks if the trigger it enters has
        *  enough space, the same item type as
        *  this, and if it is a crate.
        *  If it meets the conditions above, it
        *  will add itself to said crate.
        */
        AddToCrate(other);
    }

    //  Makes sure that anything it was assigned to is not broken.
    void OnDestroy()
    {
        //  Check if it is currently in one or more triggers.
        if (triggers.Count > 0)
        {
            //  Remove itself from each trigger's storedObjects list.
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
       /*
        *  Checks if the trigger it enters has
        *  enough space, the same item type as
        *  this, and if it is a crate.
        *  If it meets the conditions above, it
        *  will add itself to said crate.
        */
        if (other.gameObject.CompareTag("Crate") && other.gameObject.TryGetComponent<CrateScript>(out CrateScript crateScript) && !crateScript.isFull())
        {
            #region Add to Crate
            //Adds the item to the crate and resets some variables.
            crateScript.AddItem(item);
            HoverScript.selectedGameObject = null;
            GrabScript.holdingObject = false;
            Destroy(gameObject);
            #endregion 
        }
    }
    #endregion
}
