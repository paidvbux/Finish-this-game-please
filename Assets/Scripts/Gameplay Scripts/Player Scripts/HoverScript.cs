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

    public static HoverScript singleton;
    public static float MaxReach => singleton.maxReach;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        singleton = this;    
    }

    void Update()
    {
        #region Update Selected Object
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxReach, layerMask))
            selectedGameObject = hit.collider.gameObject;
        else
            selectedGameObject = null;
        #endregion
    }
    #endregion
}
