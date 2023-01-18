using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    #region Static Variables
    public static bool holdingObject;
    #endregion

    #region General Settings
    [Header("General Settings")]
    [SerializeField] LayerMask grabbableLayers;
    [SerializeField] float speed = 5f;

    [SerializeField] float rotationLerpTime;
    #endregion

    #region Hidden/Private Variables
    int storedLayerIndex;
    float desiredDistanceFromPlayer;
    Vector3 desiredObjectPosition;
    
    Rigidbody heldRigidbody;
    bool lerpingRotation;

    Vector3 rotationVelocity = Vector3.zero;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Start()
    {
        //  Resets some variables.
        holdingObject = false;
    }

    void Update()
    {
        //  Checks if the selected GameObject is not empty and is a grabbable object.
        if (HoverScript.selectedGameObject != null && (HoverScript.selectedGameObject.CompareTag("Grabbable") || HoverScript.selectedGameObject.CompareTag("Dropped Item") || HoverScript.selectedGameObject.CompareTag("Crate")))
        {
            //  Pickup the object
            PickupObject();
        }

        //  Checks if the player is holding an object already.
        if (holdingObject)
        {
            //  Checks if the player wants to drop the object.
            DropObject();

            //  Return early if they do decide to drop the object.
            if (!holdingObject) return;

            //  Check if player wants to rotate the object.
            RotateTowardsPlayer();

           /*
            * Calculate the position that 
            * the player wants the object 
            * to go to and update the 
            * velocity of the object.
            */
            CalculateDesiredPosition();
            UpdateObjectPosition();
        }
    }
    #endregion

    #region Custom Functions

   /*
    *   Determines where the player
    *   wants the object to be.
    */
    void CalculateDesiredPosition()
    {
       /*
        *   Sets the desired position of the object to be the 
        *   forward vector of the camera multiplied by the 
        *   desired distance away from the player.
        */  
        desiredObjectPosition = Camera.main.transform.forward * desiredDistanceFromPlayer + Camera.main.transform.position;
    }

   /*
    *   Checks if the player picked up the object.
    *   If the player does, change some parameters. 
    */
    void PickupObject()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //  Stores the current object.
            heldRigidbody = HoverScript.selectedGameObject.GetComponent<Rigidbody>();
            
            //  Changes the drag, layer, and stores the layer for future reference.
            storedLayerIndex = heldRigidbody.gameObject.layer;
            heldRigidbody.gameObject.layer = 6;
            heldRigidbody.drag = 3.5f;

            //  Sets the desired distance to the current distance from the player to the object.
            //  Calculate the position afterwards.
            desiredDistanceFromPlayer = Vector3.Distance(Camera.main.transform.position, HoverScript.selectedGameObject.transform.position);
            CalculateDesiredPosition();

            //  Remove gravity
            heldRigidbody.useGravity = false;
            holdingObject = true;
        }
    }

   /*
    *   If the player is holding an object
    *   and they let go of the right mouse
    *   buttton, drop the object and reset
    *   the object's rigidbody settings.
    */
    void DropObject()
    {
        if (heldRigidbody == null)
            return;
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            heldRigidbody.gameObject.layer = storedLayerIndex;
            heldRigidbody.drag = 1;
            heldRigidbody.useGravity = true;
            heldRigidbody = null;
            holdingObject = false;
        }
    }

   /*
    *   Move the object towards 
    *   the desired position.
    */
    void UpdateObjectPosition()
    {
        if (heldRigidbody == null) 
            return;

        //  Multiplies by mass so heavy objects do not slow down too much
        heldRigidbody.AddForce((desiredObjectPosition - heldRigidbody.transform.position) * speed * heldRigidbody.mass);
    }

    void RotateTowardsPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
            heldRigidbody.transform.forward = new Vector3(transform.position.x - heldRigidbody.transform.position.x,
                heldRigidbody.transform.position.y, transform.position.z - heldRigidbody.transform.position.z);
    }

    void Rotate()
    {

    }
    #endregion
}