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

    public void LoadDialogue()
    {
        for (int i = 0; i < questAcceptDialogue.Length; i++)
        {
            Debug.Log(Format(questAcceptDialogue[i]));
        }

        for (int i = 0; i < questAnswerDialogue.Length; i++)
        {
        }
    }

    public virtual string Format(string text)
    {
        return string.Format(text, coinAmount);
    }
}
