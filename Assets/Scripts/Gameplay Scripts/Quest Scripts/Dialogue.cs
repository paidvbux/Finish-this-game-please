using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string speakerName;
    public Quest selectedQuest;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public bool acceptedQuest;
    string current = "";
    #endregion

    /*******************************************************************/

    #region Custom Functions
    /*
     *   Goes through each piece of 
     *   dialogue. Pauses at the end
     *   of each line, continues when
     *   player presses space.
     */
    protected IEnumerator RunDialogue(Quest.QuestDialogue[] dialogue)
    {
        if (GameManager.dialogueActive)
            yield break;

        GameManager.ToggleDialogueUI(true, speakerName);
        for (int i = 0; i < dialogue.Length; i++)
        {
            current = "";
            StartCoroutine(CustomFunctions.LerpText(current, dialogue[i].dialogue, dialogue[i].timeBetweenCharacters, GetValue));

            if (!dialogue[i].hasResponse)
                yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
        }

        if (dialogue[dialogue.Length - 1].hasResponse)
        {
            yield return new WaitWhile(() => (current == dialogue[dialogue.Length - 1].dialogue));
            GameManager.ToggleDialogueChoices(true);
            yield return new WaitWhile(() => !GameManager.inputResponse);
            if (acceptedQuest)
                StartCoroutine(RunDialogue(selectedQuest.questAcceptDialogue));
            else
                StartCoroutine(RunDialogue(selectedQuest.questRejectDialogue));
        }

        GameManager.ToggleDialogueUI(false);
        GameManager.ToggleDialogueChoices(false);
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
        GameManager.DialogueText.text = current;
    }
    #endregion
}
