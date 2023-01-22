using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Harvest Quest", menuName = "Quests/Harvest Quest")]
public class HarvestQuest : Quest
{
    [Header("Item Settings")]
    public bool isRandom;
    public Item requiredItem;
    public int amountRequired;

    public HarvestQuest LoadQuest()
    {
        //  Randomize variables.
        if (isRandom)
            Randomize();

        //  Create a new instance of the object
        HarvestQuest quest = Instantiate(this);
        quest.name = $"{requiredItem.itemName} {amountRequired}";

        #region Format Dialogue
        // Format all the dialogue.
        for (int i = 0; i < quest.questAcceptDialogue.Length; i++)
            quest.questAcceptDialogue[i] = Format(quest.questAcceptDialogue[i]);

        for (int i = 0; i < quest.questAnswerDialogue.Length; i++)
            quest.questAnswerDialogue[i] = Format(quest.questAnswerDialogue[i]);
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
        return string.Format(text, coinAmount, amountRequired,
            amountRequired == 1 ? requiredItem.itemName : requiredItem.pluralItemName);
    }
}
