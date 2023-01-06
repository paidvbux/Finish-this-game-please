using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Scriptable Objects/Crop")]
public class Crop : ScriptableObject
{
    public string name;

    public GameObject cropObject;
    public CropItem cropItem;

    public float cropGrowTime = 30.0f;
    public Vector2 cropDimensions = new Vector2 (3, 3);
    public Vector2 cropMargins = new Vector2 (0.1f, 0.1f);
}
