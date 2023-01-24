using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Harvest Quest", menuName = "Quests/Harvest Quest")]
public class HarvestQuest : Quest
{
    #region Item Variables/Settings
    [Header("Item Settings")]
    public bool isRandom;
    public Item requiredItem;
    public int amountRequired;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public HarvestQuest LoadQuest()
    {
        #region Randomize
        //  Randomize variables.
        if (isRandom)
            Randomize();
        #endregion

        #region Create Quest
        //  Create a new instance of the object
        HarvestQuest quest = Instantiate(this);
        quest.name = $"{requiredItem.itemName} {amountRequired}";
        #endregion

        #region Format Dialogue
        // Format all the dialogue.
        for (int i = 0; i < quest.questInitalDialogue.Length; i++)
            quest.questInitalDialogue[i].dialogue = Format(quest.questInitalDialogue[i].dialogue);
        for (int i = 0; i < quest.questAcceptDialogue.Length; i++)
            quest.questAcceptDialogue[i].dialogue = Format(quest.questAcceptDialogue[i].dialogue);
        for (int i = 0; i < quest.questRejectDialogue.Length; i++)              
            quest.questRejectDialogue[i].dialogue = Format(quest.questRejectDialogue[i].dialogue);
        for (int i = 0; i < quest.questFinishDialogue.Length; i++)
            quest.questFinishDialogue[i].dialogue = Format(quest.questFinishDialogue[i].dialogue);
        #endregion

        //  Return the new quest.
        return quest;
    }

    void Randomize()
    {
        //  Randomize all variables.
        #region Randomize Variables
        int randomIndex = Random.Range(0, GameManager.items.Length);
        requiredItem = GameManager.items[randomIndex];
        amountRequired = Random.Range(requiredItem.minQuestRequirement, requiredItem.maxQuestRequirement);
        coinAmount = amountRequired * requiredItem.buyCost;
        #endregion
    }

    string Format(string text)
    {
        //  Return the formatted string.
        return string.Format(text, $"<color=#EDBC29>{coinAmount}</color>", $"<color=#428CF1>{amountRequired}</color>",
            $"<color=#428CF1>{(amountRequired == 1 ? requiredItem.itemName : requiredItem.pluralItemName)}</color>");
    }
    #endregion
}
