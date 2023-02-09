using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHolderScript : MonoBehaviour
{
    #region General Settings
    [Header("General Settings")]
    public Item heldItem;
    public Transform position;
    #endregion

    #region Hidden Variables
    Rigidbody heldObject;
    #endregion 

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        heldItem = GrabScript.holdingObject ? heldItem : null;

        #region Pickup
        if (heldObject != null && GrabScript.singleton.heldRigidbody == heldObject)
        {
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
        if (GameManager.isInteractableObject(gameObject) && Input.GetKeyDown(KeyCode.E))
        {
            GrabScript.singleton.heldRigidbody = null;
            GrabScript.holdingObject = false;

            heldObject.gameObject.layer = 0;
            heldObject.transform.position = position.position;
            heldObject.isKinematic = true;
            heldObject.useGravity = false;
        }
        #endregion
    }

    void OnTriggerEnter(Collider other)
    {
        if (GrabScript.singleton.heldRigidbody == null)
            return;

        #region Set Item
        if (other.gameObject.TryGetComponent(out DroppedItemScript heldItemScript) && GrabScript.singleton.heldRigidbody.gameObject == other.gameObject)
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
}
