using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Static Variables
    public static QuestManager singleton;

    public static Quest[] allQuests;
    public static Quest[] allRandomQuests;
    public static List<Quest> availableQuests;

    public static List<Quest> acceptedQuests = new List<Quest>();
    #endregion

    #region Quest UI Settings
    [Header("Quest UI Settings")]
    [SerializeField] GameObject questLog;
    [SerializeField] GameObject questUI;
    [SerializeField] RectTransform questListParent;
    #endregion

    #region Quest Description Settings
    [Header("Quest Description Settings")]
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] TextMeshProUGUI rewardAmount;
    [Space()]
    [SerializeField] RectTransform questObjectiveParent;
    [SerializeField] GameObject harvestObjectiveUI;
    [Space()]
    [SerializeField] RectTransform questRewardParent;
    [SerializeField] GameObject questRewardUI;
    #endregion

    #region Hidden Variables
    List<GameObject> questObjectiveObjects = new List<GameObject>();
    List<GameObject> questRewardObjects = new List<GameObject>();
    List<GameObject> questObjects = new List<GameObject>();
    #endregion

    //////////////////////////////////////////////////

    #region Unity Runtime Functions
    void Awake()
    {
        singleton = this;

        allQuests = Resources.LoadAll<Quest>("Quests");
        allRandomQuests = Resources.LoadAll<Quest>("Random Quests");

        questObjectiveObjects = new List<GameObject>();    
    }
    #endregion

    #region Custom Functions
    public void LoadQuestLog()
    {
        foreach (Quest quest in acceptedQuests)
        {
            GameObject questObject = Instantiate(questUI, questListParent);

            QuestUI questUIScript = questObject.GetComponent<QuestUI>();
            questUIScript.Initialize(quest);

            questObjects.Add(questObject);
        }

        questListParent.sizeDelta = new Vector2(questListParent.sizeDelta.x, questObjects.Count * 200f);

        questLog.SetActive(true); 
    }

    public void CloseQuestLog()
    {
        questLog.SetActive(false);

        foreach (GameObject questObjectiveObject in questObjectiveObjects)
            Destroy(questObjectiveObject);
        foreach (GameObject questRewardObject in questRewardObjects) 
            Destroy(questRewardObject);
        foreach (GameObject questObject in questObjects) 
            Destroy(questObject);

        questObjectiveObjects.Clear();
        questRewardObjects.Clear();
        questObjects.Clear();
    }

    public void LoadQuest(Quest quest)
    {
        questName.text = quest.name;
        questDescription.text = quest.description;

        LoadQuestObjectives(quest);
        LoadQuestRewards(quest);
    }

    void LoadQuestObjectives(Quest quest)
    {
        if (quest is HarvestQuest)
        {
            HarvestQuest harvestQuest = quest as HarvestQuest;
            foreach (HarvestQuest.RequiredQuestItem requiredItem in harvestQuest.requiredQuestItems)
            {
                GameObject questObjectiveUI = Instantiate(harvestObjectiveUI, questObjectiveParent);
                RequiredQuestItem requiredItemUI = questObjectiveUI.GetComponent<RequiredQuestItem>();
                requiredItemUI.Initialize(requiredItem);

                questObjectiveObjects.Add(questObjectiveUI);
            }
        }

        questObjectiveParent.sizeDelta = new Vector2(questObjectiveParent.sizeDelta.x, questObjectiveObjects.Count * 150f);
    }

    void LoadQuestRewards(Quest quest)
    {
        foreach (Quest.Reward reward in quest.rewards)
        {
            GameObject questObjectiveUI = Instantiate(questRewardUI, questRewardParent);
            QuestReward questReward = questObjectiveUI.GetComponent<QuestReward>();
            questReward.Initialize(reward.amount, reward.item);

            questRewardObjects.Add(questObjectiveUI);
        }

        questRewardParent.sizeDelta = new Vector2(questRewardParent.sizeDelta.x, questRewardObjects.Count * 150);

        rewardAmount.text = $"{quest.coinAmount}";
    }
    #endregion
}
