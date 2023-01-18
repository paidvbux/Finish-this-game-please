using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFOV : MonoBehaviour
{
    public Camera cameraToMatch;
    Camera cam => GetComponent<Camera>();

    void LateUpdate()
    {
        cam.fieldOfView = cameraToMatch.fieldOfView;
    }
}
