using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StationScript : MonoBehaviour
{
    #region General Variables
    [Header("General Settings")]
    [SerializeField] string interactText;
    [SerializeField] GameObject button;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        #region Interact
        if (GameManager.isInteractableObject(button) && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        CheckIfInteractable();
        #endregion
    }
    #endregion

    #region Custom Functions
    public abstract void Interact();
    
    
    void CheckIfInteractable()
    {
        if (HoverScript.selectedGameObject == button && !GameManager.isInteractableObject(button) && GameManager.isEmpty())
            GameManager.SetInteractableObject(interactText, button);
        else if ((HoverScript.selectedGameObject != button) && GameManager.isInteractableObject(button))
            GameManager.SetInteractableObject();
    }
    #endregion
}
