using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestReward : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI amountText;

    public void Initialize(int amount, Item item)
    {
        itemImage.sprite = item.sprite;
        nameText.text = item.itemName;
        amountText.text = $"x{amount}";
    }
}
