using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    [Header("General Settings")]
    public string questName;

    [Header("Dialogue Settings")]
    public TextAsset questDialogue;

    [Header("Reward Settings")]
    public bool isItem;
    public Item[] rewards;
    public int coinAmount;
}
