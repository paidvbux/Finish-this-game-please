using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        string path = $"Quest Dialogue/Random Harvest Dialogue/{requiredQuestItem.name} Dialogue/Random Dialogue ({amountRequired}, {coinAmount})";
        if (isRandom && Resources.Load<TextAsset>(path) == null)
        {
            string assetPath = $"Assets/Resources/{path}.txt";
            File.WriteAllText(assetPath, Format(questDialogue.text));
            AssetDatabase.Refresh();
        }
        quest.questDialogue = Resources.Load<TextAsset>(path);
        #endregion

        return quest;
    }

    void Randomize()
    {
        #region Randomize Variables
        int randomIndex = Random.Range(0, GameManager.questItems.Length);
        requiredQuestItem = GameManager.questItems[randomIndex];
        amountRequired = Random.Range(requiredQuestItem.minQuestRequirement, requiredQuestItem.maxQuestRequirement + 1);
        coinAmount = amountRequired * requiredQuestItem.buyCost;
        questName = $"Daily - Harvest {amountRequired} {requiredQuestItem}";
        #endregion
    }

    string Format(string text)
    {
        return string.Format(text, $"<color=#EDBC29>{coinAmount}</color>", $"<color=#428CF1>{amountRequired}</color>",
            $"<color=#428CF1>{(amountRequired == 1 ? requiredQuestItem.itemName : requiredQuestItem.pluralItemName)}</color>");
    }
    #endregion
}
