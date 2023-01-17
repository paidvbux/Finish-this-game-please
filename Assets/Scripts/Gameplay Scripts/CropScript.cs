using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropScript : MonoBehaviour
{
    [SerializeField] GameManager.OutlineParams outlineParameters;
    [HideInInspector] public PlotScript plotScript;
    Outline outline;

    public void Hover()
    {
        if (HoverScript.selectedGameObject == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.E)) plotScript.Harvest();
            if (outline == null)
                outline = GameManager.AddOutline(gameObject, outlineParameters);
        }
        else
        {
            if (outline != null)
            {
                Destroy(outline);
                outline = null;
            }
        }
    }
}
