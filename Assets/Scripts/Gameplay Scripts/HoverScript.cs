using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    #region General Variables/Settings
    [SerializeField] LayerMask layerMask;
    [SerializeField] float maxReach = 5f;
    #endregion

    #region Static Variables
    public static GameObject selectedGameObject;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        //  Selects the object that the raycast hits, returns null when the raycast hits nothing.
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxReach, layerMask))
            selectedGameObject = hit.collider.gameObject;
        else
            selectedGameObject = null;
    }
    #endregion
}