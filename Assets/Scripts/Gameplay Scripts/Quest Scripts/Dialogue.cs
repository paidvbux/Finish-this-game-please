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
    public TextAsset alreadyAccepted;
    public TextMeshProUGUI dialogueText;
    public GameObject hasQuest;
    public TriggerScript interactTrigger;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public bool acceptedQuest;
    [HideInInspector] public bool talking;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        acceptedQuest = false;    
    }

    protected virtual void Update()
    {
        hasQuest.SetActive(!talking && !acceptedQuest && interactTrigger.inTrigger);
    }
    #endregion

    #region Custom Functions
    public void RunDialogue(TextAsset dialogue)
    {
        if (!acceptedQuest)
        {
            DialogueLoader.singleton.speakerText = dialogueText;
            DialogueLoader.singleton.selectedQuestGiver = this;
            DialogueLoader.singleton.SetFile(dialogue);
        }
        else
        {
            DialogueLoader.singleton.speakerText = dialogueText;
            DialogueLoader.singleton.selectedQuestGiver = this;
            DialogueLoader.singleton.SetFile(alreadyAccepted);
        }
    }

    public void AcceptQuest()
    {
        QuestLoader.acceptedQuests.Add(selectedQuest);
        acceptedQuest = true;
    }
    #endregion
}
