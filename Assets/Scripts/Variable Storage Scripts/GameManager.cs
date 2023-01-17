using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [System.Serializable]
    public class OutlineParams
    {
        public Outline.Mode outlineMode = Outline.Mode.OutlineVisible;
        public Color color = Color.white;
        [Range(0, 10)] public float outlineWidth = 5;
    }

    public static List<CropScript> crops;
    public Shader litShader;

    [Header("Important Player Variables")]
    public int coins;

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