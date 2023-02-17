using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public string speakerName;
    public Quest selectedQuest;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public bool acceptedQuest;
    string current = "";
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void RunDialogue(TextAsset dialogue)
    {
        DialogueLoader.singleton.SetFile(dialogue);
    }
    #endregion
}
