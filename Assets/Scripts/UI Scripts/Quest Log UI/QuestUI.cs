using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Quest quest;

    public void Initialize(Quest _quest)
    {
        nameText.text = _quest.name;
        quest = _quest;
    }

    public void LoadQuest()
    {
        QuestManager.singleton.LoadQuest(quest);
    }
}
