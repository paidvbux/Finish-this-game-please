using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string speakerName;
    public Quest selectedQuest;
    public TextMeshProUGUI dialogueText;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public bool acceptedQuest;
    string current = "";
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void RunDialogue(TextAsset dialogue)
    {
        DialogueLoader.singleton.speakerText = dialogueText;
        DialogueLoader.singleton.SetFile(dialogue);
    }

    public void AcceptQuest()
    {
        GameManager.quests.Add(selectedQuest);
        print($"Added {selectedQuest.name} to quests");
    }
    #endregion
}
