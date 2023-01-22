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
    #endregion

    #region Sensitivity Variables/Settings
    [Header("Sensitivity Settings")]
    [SerializeField] float rotationSensitivity = 1.0f;
    [SerializeField] float scrollSensitivity = 1.0f;
    #endregion

    #region Hidden/Private Variables
    int storedLayerIndex;
    float desiredDistanceFromPlayer;
    Vector3 desiredObjectPosition;

    Rigidbody heldRigidbody;
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
        //  Prevents any bugs from occuring when talking to a NPC while holding an object.
        if (GameManager.dialogueActive)
            return;

        #region Pickup
        //  Checks if the selected GameObject is not empty and is a grabbable object.
        if (HoverScript.selectedGameObject != null && (HoverScript.selectedGameObject.CompareTag("Grabbable") || HoverScript.selectedGameObject.CompareTag("Dropped Item") || HoverScript.selectedGameObject.CompareTag("Crate")))
        {
            //  Pickup the object
            PickupObject();
        }
        #endregion

        #region Drop
        //  Checks if the player is holding an object already.
        if (holdingObject)
        {
            //  Checks if the player wants to drop the object.
            DropObject();

            //  Return early if they do decide to drop the object.
            if (!holdingObject) return;


           /*
            * Calculate the position that 
            * the player wants the object 
            * to go to and update the 
            * velocity of the object.
            */
            CalculateDesiredPosition();
            UpdateObjectPosition();
        }
        #endregion

        #region Rotate Object
        //  Check if player wants to rotate the object.
        if (holdingObject)
            Rotate();
        #endregion
    }
    #endregion

    #region Custom Functions

    /*
     *   Determines where the player
     *   wants the object to be.
     */
    void CalculateDesiredPosition()
    {
        //  Change the desired distance by scroll wheel.
        if (Input.mouseScrollDelta.y > 0)
        {
            desiredDistanceFromPlayer += scrollSensitivity * Time.deltaTime * 100;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            desiredDistanceFromPlayer -= scrollSensitivity * Time.deltaTime * 100;
        }

        //  Clamp the distance minimum distance to the max distance.
        desiredDistanceFromPlayer = Mathf.Clamp(desiredDistanceFromPlayer, 2f, HoverScript.MaxReach);

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

            #region Calculate Position
            //  Sets the desired distance to the current distance from the player to the object.
            //  Calculate the position afterwards.
            desiredDistanceFromPlayer = Vector3.Distance(Camera.main.transform.position, HoverScript.selectedGameObject.transform.position);
            CalculateDesiredPosition();
            #endregion

            #region Change Parameters
            //  Changes the drag, layer, and stores the layer for future reference.
            storedLayerIndex = heldRigidbody.gameObject.layer;
            heldRigidbody.gameObject.layer = 6;
            heldRigidbody.drag = 3.5f;

            //  Remove gravity
            heldRigidbody.useGravity = false;
            holdingObject = true;
            #endregion
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
            #region Reset Parameters
            heldRigidbody.gameObject.layer = storedLayerIndex;
            heldRigidbody.drag = 1;
            heldRigidbody.useGravity = true;
            heldRigidbody = null;
            holdingObject = false;
            #endregion
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

        //  Multiplies by mass so heavy objects do not slow down too much.
        heldRigidbody.AddForce((desiredObjectPosition - heldRigidbody.transform.position) * speed * heldRigidbody.mass * Time.deltaTime * 250);
    }

   /*
    *   Rotates the object so that
    *   it is facing the player.
    */
    void RotateTowardsPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            //  Remove any angular velocity to make sure that it does not overshoot.
            heldRigidbody.angularVelocity = Vector3.zero;

            #region Calculate Rotation
            //  Calculate the direction for the crate to look.
            Vector3 rotationDirection = transform.position - heldRigidbody.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            heldRigidbody.transform.forward = rotationDirection;
            #endregion 
        }
    }

    void Rotate()
    {
        RotateTowardsPlayer();

        #region Rotate w/ Mouse
        if (holdingObject)
        {
            //  Checks if the player is trying to rotate the object.
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //  Disables the camera movement on the PlayerController.
                PlayerController.rotatingObject = true;

                //  Gets the mouse input.
                Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

                //  Rotates the object.
                heldRigidbody.transform.Rotate(Vector3.right * targetMouseDelta.y * rotationSensitivity);
                heldRigidbody.transform.Rotate(Vector3.up * targetMouseDelta.x * rotationSensitivity);
            }
            else
            {
                //  Re-enables the camera movement on the PlayerController.
                PlayerController.rotatingObject = false;
            }
        }
    #endregion
    }
    #endregion
}