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
        GameManager.CheckIfInteractable(interactText, button);

        if (GameManager.isInteractableObject(button) && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        #endregion
    }
    #endregion

    #region Custom Functions
    public abstract void Interact();
    #endregion
}
