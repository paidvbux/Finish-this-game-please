using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLoader : MonoBehaviour
{
    #region Static Variables/Settings
    public static QuestLoader singleton;

    public static Quest[] allQuests;
    public static Quest[] allRandomQuests;
    public static List<Quest> availableQuests;
    public static List<Quest> dailyQuests;

    public static List<Quest> acceptedQuests = new List<Quest>();

    public static int dailyQuestsAvailable;
    #endregion

    #region General Variables/Settings
    public List<QuestBoardScript> questBoards;
    #endregion

    #region Hidden/Private Variables
    List<string> dailyQuestNames;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        singleton = this;

        allQuests = Resources.LoadAll<Quest>("Quests");
        allRandomQuests = Resources.LoadAll<Quest>("Random Quests");

        dailyQuestNames = new List<string>();
        dailyQuests = new List<Quest>();

        dailyQuestsAvailable = questBoards.Count;
    }
    
    void Start()
    {
        GenerateDailyQuests();
    }
    #endregion

    #region Custom Functions
    void GenerateDailyQuests()
    {
        int questsGenerated = 0;
        int attempts = 0;

        while (questsGenerated < dailyQuestsAvailable)
        {
            int randomIndex = Random.Range(0, allRandomQuests.Length - 1);
            Quest randomQuest = allRandomQuests[randomIndex];
            
            Quest generatedQuest = null;

            if (randomQuest is HarvestQuest)
            {
                Quest temp = randomQuest;
                HarvestQuest quest = randomQuest as HarvestQuest;
                generatedQuest = quest.LoadQuest();
                allRandomQuests[randomIndex] = temp;
            }

            if (attempts++ >= 10) 
                break;

            if (generatedQuest != null && !dailyQuestNames.Contains(generatedQuest.name))
            {
                dailyQuestNames.Add(generatedQuest.name);
                dailyQuests.Add(generatedQuest);
                
                questsGenerated++;

                attempts = 0;
            }

            questBoards[questsGenerated - 1].selectedQuest = generatedQuest;
        }
    }
    #endregion
}
