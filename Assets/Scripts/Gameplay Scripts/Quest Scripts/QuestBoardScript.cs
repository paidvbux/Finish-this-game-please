using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoardScript : Dialogue
{
    #region General Variables/Settings
    [Header("General Settings")]
    [SerializeField] GameObject interactableObject;
    #endregion

    /*******************************************************************/
    
    #region Unity Runtime Functions
    void Update()
    {
        #region Interact
        GameManager.CheckIfInteractable("Interact", interactableObject);
        
        if (GameManager.isInteractableObject(interactableObject) && Input.GetKeyDown(KeyCode.E))
            RunDialogue(selectedQuest.questDialogue);
        #endregion
    }
    #endregion
}
