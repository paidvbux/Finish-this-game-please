using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Harvest Quest", menuName = "Quests/Harvest Quest")]
public class HarvestQuest : Quest
{
    [System.Serializable]
    public class RequiredQuestItem
    {
        public QuestItem questItem;
        public int amountRequired;
        public int currentAmount;

        public RequiredQuestItem(QuestItem _questItem, int _amountRequired)
        {
            questItem = _questItem;
            amountRequired = _amountRequired;
            currentAmount = 0;
        }
    }

    #region Item Variables/Settings
    [Header("Item Settings")]
    public bool isRandom;
    public List<RequiredQuestItem> requiredQuestItems;
    public int minAmount = 1;
    public int maxAmount = 2;
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
        quest.name = "Random Harvest Quest";
        #endregion

        #region Format Dialogue
        string combination = "";
        string questName = "";
        foreach (RequiredQuestItem item in requiredQuestItems)
        {
            questName += $"{item.amountRequired} " + (item.amountRequired == 1 ? 
                item.questItem.itemName : item.questItem.pluralItemName) + ", ";
            combination += item.questItem.itemName + ", ";
        }

        combination = combination.Substring(0, combination.Length - 2);
        questName = questName.Substring(0, questName.Length - 2);

        string directory = $"Assets/Resources/Quest Dialogue/Random Harvest Dialogue/{combination}";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        string path = $"Quest Dialogue/Random Harvest Dialogue/{combination}/Random Dialogue [({questName}), {coinAmount}]";
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
        int cropAmount = Random.Range(minAmount, maxAmount + 1);
        requiredQuestItems = new List<RequiredQuestItem>();
        List<string> names = new List<string>();

        coinAmount = 0;
        for (int i = 0; i < cropAmount; i++)
        {
            QuestItem generatedQuestItem = null;
            
            int attempts = 0;
            while (attempts < 10)
            {
                int randomIndex = Random.Range(0, GameManager.questItems.Length);

                if (!names.Contains(GameManager.questItems[randomIndex].itemName))
                {
                    generatedQuestItem = GameManager.questItems[randomIndex];
                    names.Add(GameManager.questItems[randomIndex].itemName);
                    break;
                }
                attempts++;
            }

            if (attempts >= 10)
                break;

            int amountRequired = Random.Range(generatedQuestItem.minQuestRequirement, generatedQuestItem.maxQuestRequirement + 1);

            RequiredQuestItem questItem = new RequiredQuestItem(generatedQuestItem, amountRequired);
            requiredQuestItems.Add(questItem);

            coinAmount += amountRequired * generatedQuestItem.buyCost;
        }
        string cropName = cropAmount > 1 ? "Crops" : $"{requiredQuestItems[0].amountRequired} {(requiredQuestItems[0].amountRequired == 1 ? requiredQuestItems[0].questItem.itemName : requiredQuestItems[0].questItem.pluralItemName)}";
        questName = $"Daily - Harvest {cropName}";

        RequiredQuestItem[] tempList = new RequiredQuestItem[cropAmount];
        names.Sort();

        foreach (RequiredQuestItem requiredQuestItem in requiredQuestItems)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (requiredQuestItem.questItem.itemName == names[i])
                {
                    tempList[i] = requiredQuestItem;
                    break;
                }
            }
        }

        requiredQuestItems.Clear();
        requiredQuestItems.AddRange(tempList);
        #endregion
    }

    string Format(string text)
    {
        string itemList = "<color=#428CF1>";
        for (int i = 0; i < requiredQuestItems.Count; i++)
        {
            itemList += $"{requiredQuestItems[i].amountRequired} {(requiredQuestItems[i].amountRequired == 1 ? requiredQuestItems[i].questItem.itemName : requiredQuestItems[i].questItem.pluralItemName)}";
            if (i + 1 < requiredQuestItems.Count)
                itemList += ", ";
        }
        itemList += "</color>";

        return string.Format(text, $"<color=#EDBC29>{coinAmount}</color>", itemList);
    }
    #endregion
}
