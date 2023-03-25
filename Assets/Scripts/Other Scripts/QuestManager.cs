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
    [SerializeField] TriggerScript selectedTrigger;
    [SerializeField] Quest selectedQuest;

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
    [HideInInspector] public RedeemStationScript selectedRedeemStation;

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

    [HideInInspector] public bool open;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!open)
            {
                LoadQuestLog();
            }
            else
            {
                CloseQuestLog();
            }
        }
    }
    #endregion

    #region Custom Functions
    #region Public Functions
    #region Public UI Functions
    public void LoadQuestLog()
    {
        GameManager.uiActive = true;
        open = true;

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
        GameManager.uiActive = false;
        open = false;

        if (selectedRedeemStation != null)
        {
            selectedRedeemStation.selectedQuest = null;
            selectedRedeemStation = null;
        }

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
    #endregion
    
    public void RedeemItems(TriggerScript trigger, Quest quest)
    {
        selectedTrigger = trigger;
        selectedQuest = quest;

        foreach (HarvestQuest.RequiredQuestItem item in (selectedQuest as HarvestQuest).requiredQuestItems)
        {
            if (item.currentAmount == item.amountRequired)
                continue;

            int amountNeeded = item.currentAmount - item.amountRequired;

            foreach (KeyValuePair<GameObject, string> value in selectedTrigger.storedObjects)
            {
                if (value.Value == "Crate" && value.Key.TryGetComponent(out CrateScript crate) && item.questItem == crate.storedItem)
                {
                    int amount = Mathf.Min(amountNeeded, crate.storedAmount);

                    item.currentAmount += amount;
                    crate.storedAmount -= amount;

                    if (crate.storedAmount == 0)
                        crate.storedItem = null;
                }
                else if (value.Value == "Dropped Item")
                {
                    DroppedItemScript droppedItem = value.Key.GetComponent<DroppedItemScript>();
                    if (item.questItem != droppedItem.item)
                        continue;
                    item.currentAmount++;
                    Destroy(value.Key);
                }
            }
        }
    }
    #endregion
    
    #region UI Functions
    public void LoadQuest(Quest quest)
    {
        if (selectedRedeemStation != null)
            selectedRedeemStation.selectedQuest = quest;

        questName.text = quest.questName;
        questDescription.text = quest.description;

        LoadQuestObjectives(quest);
        LoadQuestRewards(quest);
    }

    void LoadQuestObjectives(Quest quest)
    {
        foreach (GameObject questObjectiveObject in questObjectiveObjects)
            Destroy(questObjectiveObject);
        questObjectiveObjects.Clear();

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
        foreach (GameObject questRewardObject in questRewardObjects)
            Destroy(questRewardObject);
        questRewardObjects.Clear();

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
    #endregion
}
