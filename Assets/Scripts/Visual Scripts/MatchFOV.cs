using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFOV : MonoBehaviour
{
    #region General Variables/Settings
    public Camera cameraToMatch;
    #endregion
    
    #region Hidden/Private Variables
    Camera cam => GetComponent<Camera>();
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void LateUpdate()
    {
        //  Matches the FOV of the two cameras.
        cam.fieldOfView = cameraToMatch.fieldOfView;
    }
    #endregion
}
