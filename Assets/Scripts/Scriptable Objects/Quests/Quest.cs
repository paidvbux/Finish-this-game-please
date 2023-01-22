using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{
    public string questName;

    [Header("Dialogue")]
    public string[] questAcceptDialogue;
    public string[] questAnswerDialogue;

    [Header("Reward")]
    public bool isItem;
    public Item item;
    public int coinAmount;
}
