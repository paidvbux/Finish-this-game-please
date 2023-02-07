using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Item", menuName = "Item/Item")]
public class QuestItem : Item
{
    #region Quest Variables/Settings
    [Header("Quest Settings")]
    public int minQuestRequirement;
    public int maxQuestRequirement;
    #endregion
}
