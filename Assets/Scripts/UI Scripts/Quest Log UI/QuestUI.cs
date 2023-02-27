using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    [HideInInspector] public Quest quest;

    public void Initialize(Quest _quest)
    {
        nameText.text = _quest.questName;
        quest = _quest;
    }

    public void LoadQuest()
    {
        QuestManager.singleton.LoadQuest(quest);
    }
}
