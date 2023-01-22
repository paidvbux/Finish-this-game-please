using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLoader : MonoBehaviour
{
    #region Static Variables/Settings
    public static Quest[] allQuests;
    public static Quest[] allRandomQuests;
    public static List<Quest> availableQuests;
    public static List<Quest> dailyQuests;

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
        //  Load all the quests.
        allQuests = Resources.LoadAll<Quest>("Quests");
        allRandomQuests = Resources.LoadAll<Quest>("Random Quests");

        //  Initialize lists.
        dailyQuestNames = new List<string>();

        dailyQuestsAvailable = questBoards.Count;
    }
    
    void Start()
    {
        GenerateDailyQuests();
    }
    #endregion

    #region Custom Functions
   /*
    *   Used every morning to generate
    *   new daily quests for the player
    *   to do.
    */
    void GenerateDailyQuests()
    {
        int questsGenerated = 0;
        int attempts = 0;

        //  Creates x amount of daily quests where x is the dailyQuestsAvailable variable.
        while (questsGenerated < dailyQuestsAvailable)
        {
            //  Gets a random type of random quest for the player to complete.
            Quest randomQuest = allRandomQuests[Random.Range(0, allRandomQuests.Length - 1)];
            
            //  Sets up a generated quest variable to store the new random quest.
            Quest generatedQuest = null;

            //  Checks for the random quest type.
            if (randomQuest is HarvestQuest)
            {
                //  Generates the quest.
                HarvestQuest quest = randomQuest as HarvestQuest;
                generatedQuest = quest.LoadQuest();
            }

            //  Makes sure that the while loop does not run forever.
            if (attempts++ >= 10) 
                break;

            //  Adds the quest to the daily quests if the generated quest is not empty.
            if (generatedQuest != null && !dailyQuestNames.Contains(generatedQuest.name))
            {
                dailyQuestNames.Add(generatedQuest.name);
                dailyQuests.Add(generatedQuest);
                
                //  Increase the amount of quests generated.
                questsGenerated++;

                //  Reset the amount of attempts.
                attempts = 0;
            }
        }
    }
    #endregion
}
