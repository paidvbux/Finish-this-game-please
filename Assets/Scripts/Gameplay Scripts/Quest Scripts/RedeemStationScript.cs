using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemStationScript : StationScript
{
    #region Trigger Settings
    [SerializeField] TriggerScript trigger;
    #endregion

    #region Hidden Variables
    [HideInInspector] public Quest selectedQuest;
    #endregion

    //////////////////////////////////////////////////

    #region Custom Functions
    public override void Interact()
    {
        StartCoroutine(RedeemItems());
    }

    IEnumerator RedeemItems()
    {
        QuestManager.singleton.LoadQuestLog();

        QuestManager.singleton.selectedRedeemStation = this;
        yield return new WaitWhile(() => selectedQuest == null);
        QuestManager.singleton.RedeemItems(trigger, selectedQuest);

        QuestManager.singleton.CloseQuestLog();
    }
    #endregion
}
