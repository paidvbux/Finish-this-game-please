using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPacketScript : GrabbableObjectScript
{
    #region Crop Settings
    [Header("Crop Settings")]
    public LayerMask layerMask;

    public Crop cropToPlant;
    public int seedsLeft;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        CheckForHighlight();
        if (GrabScript.singleton.heldRigidbody == rigidBody)
        {
            GameManager.SetInteractableObject("Plant", gameObject);

            if (Input.GetKeyDown(KeyCode.E))
                PlantSeed();
        }
        else if (GameManager.isInteractableObject(gameObject))
            GameManager.SetInteractableObject();
    }
    #endregion

    #region Custom Functions
    void PlantSeed()
    {
        if (seedsLeft <= 0)
            return;

        RaycastHit hit;
        bool hitObject = Physics.Raycast(transform.position, Vector3.down, out hit, 5, layerMask);
        bool isPlot = hit.collider.TryGetComponent(out PlotScript plot) && plot.plantedCrop == null;
        if (hitObject && isPlot)
        {
            plot.PlantSeed(cropToPlant);
            seedsLeft--;
        }
    }
    #endregion
}
