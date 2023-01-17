using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : GrabbableObjectScript
{
    [Header("Item Settings")]
    public Item item;

    void Start()
    {
        rigidBody.mass = item.mass;
        gameObject.tag = "Dropped Item";
        Initialize();
    }

    void Update()
    {
        CheckForHighlight();
    }

    void OnTriggerEnter(Collider other)
    {
        AddToCrate(other);
    }

    public void AddToCrate(Collider other)
    {
        if (other.gameObject.CompareTag("Crate") && other.gameObject.TryGetComponent<CrateScript>(out CrateScript crateScript) && !crateScript.isFull())
        {
            crateScript.AddItem(item);
            HoverScript.selectedGameObject = null;
            GrabScript.holdingObject = false;
            Destroy(gameObject);
        }
    }
}
