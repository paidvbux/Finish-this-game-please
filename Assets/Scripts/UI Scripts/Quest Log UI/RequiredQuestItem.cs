using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RequiredQuestItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI countText;

    public void Initialize(HarvestQuest.RequiredQuestItem requiredItem)
    {
        image.sprite = requiredItem.questItem.sprite;
        nameText.text = requiredItem.questItem.itemName;
        countText.text = $"<color=#17A589>{requiredItem.currentAmount}/{requiredItem.amountRequired}</color>";
    }
}
