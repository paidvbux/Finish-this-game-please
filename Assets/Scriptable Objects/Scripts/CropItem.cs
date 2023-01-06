using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop Item", menuName = "Scriptable Objects/Crop Item")]
public class CropItem : ScriptableObject
{
    public int minHarvestCount;
    public int maxHarvestCount;

    public int inventoryCount;
}
