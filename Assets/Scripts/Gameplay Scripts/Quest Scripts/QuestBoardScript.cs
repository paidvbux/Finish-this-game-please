using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoardScript : Dialogue
{
    [SerializeField] GameObject interactableObject;

    #region Unity Runtime Functions
    void Update()
    {
        if (HoverScript.selectedGameObject == interactableObject && !GameManager.isInteractableObject(interactableObject))
        {

        }
    }
    #endregion
}
