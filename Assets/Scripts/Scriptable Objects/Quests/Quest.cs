using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    [Serializable]
    public class QuestDialogue
    {
        [Multiline(4)]
        public string dialogue = "...";
        public float timeBetweenCharacters = 0.05f;

        [Header("Response Settings")]
        public bool hasResponse = false;
    }

    public string questName;

    [Header("Dialogue")]
    public QuestDialogue[] questInitalDialogue;
    public QuestDialogue[] questAcceptDialogue;
    public QuestDialogue[] questRejectDialogue;
    public QuestDialogue[] questFinishDialogue;

    [Header("Reward")]
    public bool isItem;
    public Item item;
    public int coinAmount;
}
