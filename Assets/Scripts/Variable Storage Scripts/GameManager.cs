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

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        //  Sets the singleton to this.
        singleton = this;

        //  Initializes the list of crops.
        crops = new List<CropScript>();
    }
    #endregion

    #region Custom Functions
   /*
    *   Adds an outline to the specified object.
    *   The outlines settings can be tweaked from
    *   when it is called.
    */
    public static Outline AddOutline(GameObject obj, OutlineParams outlineParams)
    {
        //  Adds the component.
        Outline outline = obj.AddComponent<Outline>();

        //  Changes the settings of the outline.
        outline.OutlineMode = outlineParams.outlineMode;
        outline.OutlineColor = outlineParams.color;
        outline.OutlineWidth = outlineParams.outlineWidth;

        //  Returns the component.
        return outline;
    }
    #endregion
}