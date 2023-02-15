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
    public QuestItem requiredQuestItem;
    public int amountRequired;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public HarvestQuest LoadQuest()
    {
        #region Randomize
        if (isRandom)
            Randomize();
        #endregion

        #region Create Quest
        HarvestQuest quest = Instantiate(this);
        quest.name = $"{requiredQuestItem.itemName} {amountRequired}";
        #endregion

        #region Format Dialogue
        for (int i = 0; i < quest.questInitalDialogue.Length; i++)
            quest.questInitalDialogue[i].dialogue = Format(quest.questInitalDialogue[i].dialogue);
        for (int i = 0; i < quest.questAcceptDialogue.Length; i++)
            quest.questAcceptDialogue[i].dialogue = Format(quest.questAcceptDialogue[i].dialogue);
        for (int i = 0; i < quest.questRejectDialogue.Length; i++)              
            quest.questRejectDialogue[i].dialogue = Format(quest.questRejectDialogue[i].dialogue);
        for (int i = 0; i < quest.questFinishDialogue.Length; i++)
            quest.questFinishDialogue[i].dialogue = Format(quest.questFinishDialogue[i].dialogue);
        #endregion

        return quest;
    }

    void Randomize()
    {
        #region Randomize Variables
        int randomIndex = Random.Range(0, GameManager.questItems.Length);
        requiredQuestItem = GameManager.questItems[randomIndex];
        amountRequired = Random.Range(requiredQuestItem.minQuestRequirement, requiredQuestItem.maxQuestRequirement);
        coinAmount = amountRequired * requiredQuestItem.buyCost;
        #endregion
    }

    string Format(string text)
    {
        return string.Format(text, $"<color=#EDBC29>{coinAmount}</color>", $"<color=#428CF1>{amountRequired}</color>",
            $"<color=#428CF1>{(amountRequired == 1 ? requiredQuestItem.itemName : requiredQuestItem.pluralItemName)}</color>");
    }
    #endregion
}
