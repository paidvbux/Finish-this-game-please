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

    #region Trigger Variables/Settings
    [Header("Trigger Setting")]
    [SerializeField] TriggerScript playerTrigger;
    [SerializeField] bool useTrigger;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        #region Interact
        if (useTrigger)
        {
            if (playerTrigger.inTrigger)
            {
                GameManager.interactableObject.gameObject = gameObject;
                GameManager.interactableObject.text = interactText;
                GameManager.interactUI.SetActive(true);
            }
            else if (GameManager.interactableObject.gameObject == gameObject)
                GameManager.interactableObject.gameObject = null;
        }
        else if (!useTrigger)
        {
            GameManager.CheckIfInteractable(interactText, button);
        }    

        bool canInteract = useTrigger ? playerTrigger.inTrigger : GameManager.isInteractableObject(button);
        if (canInteract && Input.GetKeyDown(KeyCode.E))
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
