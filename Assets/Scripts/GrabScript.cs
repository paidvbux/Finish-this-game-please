using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    public static bool holdingObject;

    [SerializeField] float maxGrabDistance = 5f;
    [SerializeField] LayerMask grabbableLayers;

    [SerializeField] float speed = 5f;

    int storedLayerIndex;
    float desiredDistanceFromPlayer;
    Vector3 desiredObjectPosition;

    Rigidbody heldRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        holdingObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HoverScript.selectedGameObject != null && (HoverScript.selectedGameObject.CompareTag("Grabbable") || HoverScript.selectedGameObject.CompareTag("Dropped Item") || HoverScript.selectedGameObject.CompareTag("Crate")))
        {
            PickupObject();
        }

        if (holdingObject)
        {
            DropObject();
            if (!holdingObject) return;

            CalculateDesiredPosition();
            UpdateObjectPosition();
        }
    }

    void CalculateDesiredPosition()
    {
        desiredObjectPosition = Camera.main.transform.forward * desiredDistanceFromPlayer + Camera.main.transform.position;
    }

    void PickupObject()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            heldRigidbody = HoverScript.selectedGameObject.GetComponent<Rigidbody>();
            storedLayerIndex = heldRigidbody.gameObject.layer;
            heldRigidbody.gameObject.layer = 6;
            heldRigidbody.drag = 3.5f;

            desiredDistanceFromPlayer = Vector3.Distance(Camera.main.transform.position, HoverScript.selectedGameObject.transform.position);
            CalculateDesiredPosition();

            heldRigidbody.useGravity = false;
            holdingObject = true;
        }
    }

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

    void UpdateObjectPosition()
    {
        if (heldRigidbody == null) 
            return;
        heldRigidbody.AddForce((desiredObjectPosition - heldRigidbody.transform.position) * speed * heldRigidbody.mass);
    }

}