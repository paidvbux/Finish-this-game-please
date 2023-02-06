using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropScript : MonoBehaviour
{
    #region General Variables/Settings
    [SerializeField] GameManager.OutlineParams outlineParameters;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public PlotScript plotScript;
    Outline outline;
    #endregion

    /*******************************************************************/

    #region Custom Functions
    public void Hover()
    {
        GameManager.CheckIfInteractable("Harvest", gameObject);
        if (HoverScript.selectedGameObject == gameObject)
        {
            #region Harvest Crop
            if (Input.GetKeyDown(KeyCode.E)) 
                plotScript.Harvest();
            #endregion

            #region Add Outline
            //  Prevents adding multiple outlines.
            if (outline == null)
                outline = GameManager.AddOutline(gameObject, outlineParameters);
            #endregion
        }
        else
        {
            #region Delete Outline
            if (outline != null)
            {
                Destroy(outline);
                outline = null;
            }
            #endregion
        }
    }
    #endregion
}
