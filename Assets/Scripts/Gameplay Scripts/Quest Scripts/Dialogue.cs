using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string speakerName;
    public string[] dialogue;
    #endregion

    #region Hidden/Private 
    public string current;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    /*
     *   Goes through each piece of 
     *   dialogue. Pauses at the end
     *   of each line, continues when
     *   player presses space.
     */
    protected IEnumerator RunDialogue()
    {
        if (GameManager.dialogueActive)
            yield break;

        GameManager.ToggleDialogueUI(true, speakerName);
        for (int i = 0; i < dialogue.Length; i++)
        {
            current = "";
            StartCoroutine(CustomFunctions.LerpText(current, dialogue[i], 0.1f, GetValue));
            yield return new WaitWhile (() => !Input.GetKeyDown(KeyCode.Space)) ;
        }
        GameManager.ToggleDialogueUI(false);
        yield break;
    }

   /*
    *   Creates the callback to allow
    *   the coroutine to return the
    *   value.
    */
    void GetValue(string value)
    {
        current = value;
    }
    #endregion
}
