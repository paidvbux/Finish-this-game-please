using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    #region Static Variables
    public static GrabScript singleton;
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

    [HideInInspector] public Rigidbody heldRigidbody;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Start()
    {
        singleton = this;

        holdingObject = false;
    }

    void Update()
    {
        if (GameManager.uiActive)
            return;

        #region Pickup
        if (HoverScript.selectedGameObject != null && (HoverScript.selectedGameObject.CompareTag("Grabbable") || HoverScript.selectedGameObject.CompareTag("Dropped Item") || HoverScript.selectedGameObject.CompareTag("Crate")))
        {
            PickupObject();
        }
        #endregion

        #region Drop
        if (holdingObject)
        {
            DropObject();

            if (!holdingObject) return;

            CalculateDesiredPosition();
            UpdateObjectPosition();
        }
        #endregion

        #region Rotate Object
        if (holdingObject)
            Rotate();
        #endregion
    }
    #endregion

    #region Custom Functions

    void CalculateDesiredPosition()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            desiredDistanceFromPlayer += scrollSensitivity * Time.deltaTime * 100;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            desiredDistanceFromPlayer -= scrollSensitivity * Time.deltaTime * 100;
        }

        desiredDistanceFromPlayer = Mathf.Clamp(desiredDistanceFromPlayer, 2f, HoverScript.MaxReach);

        desiredObjectPosition = Camera.main.transform.forward * desiredDistanceFromPlayer + Camera.main.transform.position;
    }

    void PickupObject()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            heldRigidbody = HoverScript.selectedGameObject.GetComponent<Rigidbody>();

            #region Calculate Position
            desiredDistanceFromPlayer = Vector3.Distance(Camera.main.transform.position, HoverScript.selectedGameObject.transform.position);
            CalculateDesiredPosition();
            #endregion

            #region Change Parameters
            storedLayerIndex = heldRigidbody.gameObject.layer;
            heldRigidbody.gameObject.layer = 6;
            heldRigidbody.drag = 3.5f;

            heldRigidbody.useGravity = false;
            holdingObject = true;
            #endregion
        }
    }

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

    void UpdateObjectPosition()
    {
        if (heldRigidbody == null) 
            return;

        heldRigidbody.AddForce((desiredObjectPosition - heldRigidbody.transform.position) * speed * heldRigidbody.mass * Time.deltaTime * 250);
    }

    void RotateTowardsPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            heldRigidbody.angularVelocity = Vector3.zero;

            #region Calculate Rotation
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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                PlayerController.rotatingObject = true;

                Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * rotationSensitivity;

                Vector3 translatedRight = heldRigidbody.transform.InverseTransformDirection(Vector3.right);
                Vector3 translatedUp = heldRigidbody.transform.InverseTransformDirection(Vector3.up);

                heldRigidbody.transform.Rotate(translatedRight * targetMouseDelta.y);
                heldRigidbody.transform.Rotate(translatedUp * targetMouseDelta.x);
            }
            else
                PlayerController.rotatingObject = false;
        }
        #endregion
    }
    #endregion
}