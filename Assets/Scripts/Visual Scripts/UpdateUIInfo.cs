using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpdateUIInfo : MonoBehaviour
{
    #region Classes
    [System.Serializable]
    public class ImageInfo
    {
        public string refernceName;

        public Image img;
        public Sprite sprite;
        public Color color = Color.white;

        [HideInInspector] public Sprite oldSprite;
        [HideInInspector] public Color oldColor;

       /*
        *   Checks if the value of the variable 
        *   has changed, if so change it and set
        *   the old one to the new one.
        */
        public void UpdateChanges()
        {
            if (oldSprite != sprite)
            {
                img.sprite = sprite;
                oldSprite = sprite;
            }
            if (oldColor != color)
            {
                img.color = color;
                oldColor = color;
            }
        }

        public void ChangeValue(Sprite _sprite, Color _color)
        {
            sprite = _sprite;
            color = _color;
        }
    }

    [System.Serializable]
    public class TMPInfo
    {
        public string name;

        public TextMeshProUGUI text;
        public Color color = Color.white;
        public string info;

        [HideInInspector] public Color oldColor;
        [HideInInspector] public string oldInfo;

       /*
        *   Checks if the value of the variable 
        *   has changed, if so change it and set
        *   the old one to the new one.
        */
        public void UpdateChanges()
        {
            if (oldColor != color)
            {
                text.color = color;
                oldColor = color;
            }
            if (oldInfo != info)
            {
                text.text = info;
                oldInfo = info;
            }
        }

        public void ChangeValue(Color _color, string _info)
        {
            color = _color;
            info = _info;
        }
    }
    #endregion

    #region General Variables/Settings
    [Header("General Settings")]
    public ImageInfo[] imagesToUpdate;
    public TMPInfo[] textToUpdate;

    public UIToQuadScript quadDisplay;
    #endregion

    /*******************************************************************/

    #region Custom Functions
   /*  
    *   Updates all UI elements. 
    */
    public void UpdateUIElements()
    {
        foreach (ImageInfo imageInfo in imagesToUpdate)
        {
            imageInfo.UpdateChanges();
        }

        foreach (TMPInfo textInfo in textToUpdate)
        {
            textInfo.UpdateChanges();
        }
    }
    #endregion
}
