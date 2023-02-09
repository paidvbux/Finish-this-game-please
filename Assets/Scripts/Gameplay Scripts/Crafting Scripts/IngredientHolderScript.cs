using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHolderScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    public Item heldItem;
    #endregion

    #region Hidden Variables
    Rigidbody heldObject;
    #endregion 

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        heldItem = GrabScript.holdingObject ? heldItem : null;

        if (heldItem != null && !GameManager.isInteractableObject(gameObject))
            GameManager.SetInteractableObject("Place Item", gameObject);
        else if (heldItem == null && GameManager.isInteractableObject(gameObject))
            GameManager.SetInteractableObject();
    
        if (GameManager.isInteractableObject(gameObject) && Input.GetKeyDown(KeyCode.E))
        {
            GrabScript.singleton.heldRigidbody = null;
            GrabScript.holdingObject = false;

            heldObject.transform.position = transform.position;
            heldObject.isKinematic = true;
            heldObject.useGravity = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out DroppedItemScript heldItemScript) && GrabScript.singleton.heldRigidbody.gameObject == other.gameObject)
        {
            heldObject = GrabScript.singleton.heldRigidbody;
            heldItem = heldItemScript.item;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (heldItem != null && GrabScript.singleton.heldRigidbody.gameObject == other.gameObject)
        {
            heldObject.isKinematic = false;
            heldObject.useGravity = true;

            heldObject = null;
            heldItem = null;
        }
    }
    #endregion
}
