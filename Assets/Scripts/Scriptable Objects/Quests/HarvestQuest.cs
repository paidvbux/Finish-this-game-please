using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Harvest Quest", menuName = "Quests/Harvest Quest")]
public class HarvestQuest : Quest
{
    [Header("Item Settings")]
    public bool isRandom;
    public Item requiredItem;
    public int amountRequired;

    void Randomize()
    {
        int randomIndex = Random.Range(0, GameManager.items.Length);
        requiredItem = GameManager.items[randomIndex];
        amountRequired = Random.Range(requiredItem.minQuestRequirement, requiredItem.maxQuestRequirement);
        coinAmount = amountRequired * Random.Range(2, 5);
    }

    public override string Format(string text)
    {
        if (isRandom)
            Randomize();
        Debug.Log(string.Format(text, coinAmount, amountRequired,
            amountRequired == 1 ? requiredItem.itemName : requiredItem.pluralItemName));
        return string.Format(text, coinAmount, amountRequired,
            amountRequired == 1 ? requiredItem.itemName : requiredItem.pluralItemName);
    }
}
