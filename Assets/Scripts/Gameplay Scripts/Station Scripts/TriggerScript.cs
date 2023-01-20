using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public bool inTrigger;
    [SerializeField] string[] tagsToCheck = new string[] { "Player" };
    #endregion

    #region Object Storage Settings
    [Header("Object Storage Settings")]
    public bool storeObjects;
    public Dictionary<GameObject, string> storedObjects;
    #endregion

    #region Hidden/Private Variables
    int numObjectsInTrigger;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        //  Initializes the dictionary.
        storedObjects = new Dictionary<GameObject, string>();
    }

    void Update()
    {
        //  Sets the boolean to be true if there are more than one thing in its trigger.
        inTrigger = numObjectsInTrigger > 0;
    }

    void OnTriggerEnter(Collider other)
    {
        #region Add Items/Crates
        //  Check each tag for enter collision.
        foreach (string tagToCheck in tagsToCheck) 
        { 
            if (other.gameObject.CompareTag(tagToCheck))
            {
                //  Crates have two colliders, make it only register one.
                if (tagToCheck == "Crate" && storedObjects.ContainsKey(other.gameObject))
                    continue;

                //  Stores the objects in the dictionary with the tag along with it.
                if (storeObjects)
                    storedObjects.Add(other.gameObject, other.tag);

                //  Add itself to the objects' stored triggers.
                if (tagToCheck == "Dropped Item")
                    other.gameObject.GetComponent<DroppedItemScript>().triggers.Add(this);

                //  Increase the amount of objects in the trigger by one.
                numObjectsInTrigger++;
            }
        }
        #endregion
    }

    void OnTriggerExit(Collider other)
    {
        #region Remove Items/Crates
        //  Check each tag for exit collision.
        foreach (string tagToCheck in tagsToCheck)
        {
            if (other.gameObject.CompareTag(tagToCheck))
            {
                //  Crates have two colliders, make it only register one.
                if (tagToCheck == "Crate" && !storedObjects.ContainsKey(other.gameObject))
                    continue;

                //  Removes the objects from the dictionary.
                if (storeObjects)
                    storedObjects.Remove(other.gameObject);

                //  Remove itself to the objects' stored triggers.
                if (tagToCheck == "Dropped Item")
                    other.gameObject.GetComponent<DroppedItemScript>().triggers.Remove(this);

                //  Decreases the amount of objects in the trigger by one.
                numObjectsInTrigger--;
            }
        }
        #endregion
    }
    #endregion
}
