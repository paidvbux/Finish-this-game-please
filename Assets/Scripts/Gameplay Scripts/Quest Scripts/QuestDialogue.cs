using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDialogue : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string speakerName;
    public string[] dialogue;
    #endregion

    #region Hidden/Private 
    public string current;
    #endregion

    void Start()
    {
        StartCoroutine(RunDialogue());
    }

    IEnumerator RunDialogue()
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            current = "";
            StartCoroutine(CustomFunctions.LerpText(current, dialogue[i], 0.1f, GetValue));
            yield return new WaitWhile (() => !Input.GetKeyDown(KeyCode.Space)) ;
        }
        yield break;
    }

    void GetValue(string value)
    {
        current = value;
    }
}
