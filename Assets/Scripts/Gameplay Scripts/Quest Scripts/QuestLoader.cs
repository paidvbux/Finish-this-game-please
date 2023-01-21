using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLoader : MonoBehaviour
{
    #region Static Variables/Settings
    public Quest[] quests;
    #endregion

    #region Unity Runtime Functions
    void Start()
    {
        quests = Resources.LoadAll<Quest>("Quests");

    }
    
    void Update()
    {
        //quests[0].LoadDialogue();
    }
    #endregion
}
