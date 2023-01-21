using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCropScript : DroppedItemScript
{
    #region Unity Runtime Functions
    void Start()
    {
        #region Initialization
        //  Sets the mass of the object
        rigidBody.mass = item.mass;

        //  Sets the tag to "Dropped Item" to prevent any bugs
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
    #endregion
}
