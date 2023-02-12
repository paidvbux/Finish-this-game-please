using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHolderScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    public Item storedItem;
    public Transform position;
    #endregion

    #region Hidden Variables
    Rigidbody heldObject;
    Item heldItem;
    #endregion 

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        heldItem = GrabScript.holdingObject ? heldItem : null;

        #region Pickup
        if (heldObject != null && GrabScript.singleton.heldRigidbody == heldObject)
        {
            storedItem = null;

            heldObject.gameObject.layer = 6;
            heldObject.isKinematic = false;
        }
        #endregion

        #region Change UI
        if (heldItem != null && !GameManager.isInteractableObject(gameObject))
            GameManager.SetInteractableObject("Place Item", gameObject);
        else if (heldItem == null && GameManager.isInteractableObject(gameObject))
            GameManager.SetInteractableObject();
        #endregion

        #region Place Item
        if (GameManager.isInteractableObject(gameObject) && Input.GetKeyDown(KeyCode.E) && storedItem == null)
        {
            GrabScript.singleton.heldRigidbody = null;
            GrabScript.holdingObject = false;

            storedItem = heldItem;

            heldObject.gameObject.layer = 0;
            heldObject.isKinematic = true;
            heldObject.useGravity = false;

            heldObject.transform.position = position.position + heldItem.placedOffset;
            heldObject.transform.localEulerAngles = heldItem.placedRotation;
        }
        #endregion

    }

    void OnTriggerEnter(Collider other)
    {
        if (GrabScript.singleton.heldRigidbody == null)
            return;

        #region Set Item
        if (storedItem == null && other.gameObject.TryGetComponent(out DroppedItemScript heldItemScript) && GrabScript.singleton.heldRigidbody.gameObject == other.gameObject)
        {
            heldObject = GrabScript.singleton.heldRigidbody;
            heldItem = heldItemScript.item;
        }
        #endregion
    }

    void OnTriggerExit(Collider other)
    {
        if (GrabScript.singleton.heldRigidbody == null)
            return;

        #region Remove Item
        if (heldItem != null && GrabScript.singleton.heldRigidbody.gameObject == other.gameObject)
        {
            heldObject = null;
            heldItem = null;
        }
        #endregion
    }
    #endregion

    #region Custom Functions
    public void Clear()
    {
        if (storedItem == null)
            return;

        Destroy(heldObject.gameObject);
        storedItem = null;
        heldItem = null;
        heldObject = null;
    }
    #endregion
}
