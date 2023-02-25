using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    [Serializable]
    public class Reward
    {
        public Item item;
        public int amount;
    }

    [Header("General Settings")]
    public string questName;

    [Header("Dialogue Settings")]
    public TextAsset questDialogue;

    [Header("Reward Settings")]
    public bool isItem;
    public Reward[] rewards;
    public bool hasRecipeReward;
    public Recipe[] recipeRewards;
    public int coinAmount;
}
