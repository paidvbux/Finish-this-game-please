using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCropScript : GrabbableObjectScript
{
    [Header("Crop Settings")]
    public Crop crop;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.mass = crop.mass;
        gameObject.tag = "Dropped Item";
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForHighlight();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crate") && other.gameObject.TryGetComponent<CrateScript>(out CrateScript crateScript) && !crateScript.isFull())
        {
            crateScript.AddItem(crop);
            HoverScript.selectedGameObject = null;
            GrabScript.holdingObject = false;
            Destroy(gameObject);
        }
    }
}
