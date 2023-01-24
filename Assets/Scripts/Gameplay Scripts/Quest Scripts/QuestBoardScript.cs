using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoardScript : Dialogue
{
    #region General Variables/Settings
    [SerializeField] GameObject interactableObject;
    #endregion

    #region Unity Runtime Functions
    void Update()
    {
        #region Interact
        //  Checks if the player is trying to interact with the board.
        if (HoverScript.selectedGameObject == interactableObject && !GameManager.isInteractableObject(interactableObject))
            //  Sets the current interactable object to this.
            GameManager.SetInteractableObject("Chat", interactableObject);
        else if (HoverScript.selectedGameObject != interactableObject && GameManager.isInteractableObject(interactableObject))
            //  Removes the interactable object.
            GameManager.SetInteractableObject();

        //  Checks if the player interacted with the board.
        if (GameManager.isInteractableObject(interactableObject) && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(RunDialogue(selectedQuest.questInitalDialogue));
        #endregion
    }
    #endregion
}
