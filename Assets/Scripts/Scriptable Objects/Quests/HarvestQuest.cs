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
}
