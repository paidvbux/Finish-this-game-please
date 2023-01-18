using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Classes
    [System.Serializable]
    public class OutlineParams
    {
        public Outline.Mode outlineMode = Outline.Mode.OutlineVisible;
        public Color color = Color.white;
        [Range(0, 10)] public float outlineWidth = 5;
    }
    #endregion

    #region Static Variables
    public static GameManager singleton;
    public static Transform Player => GameManager.singleton.player;
    public static List<CropScript> crops;
    #endregion

    #region Other Variables
    [Header("Important Player Variables")]
    public Transform player;
    public int coins;
    #endregion

    void Awake()
    {
        singleton = this;
        crops = new List<CropScript>();
    }

    public static Outline AddOutline(GameObject obj, OutlineParams outlineParams)
    {
        Outline outline = obj.AddComponent<Outline>();
        outline.OutlineMode = outlineParams.outlineMode;
        outline.OutlineColor = outlineParams.color;
        outline.OutlineWidth = outlineParams.outlineWidth;
        return outline;
    }

}