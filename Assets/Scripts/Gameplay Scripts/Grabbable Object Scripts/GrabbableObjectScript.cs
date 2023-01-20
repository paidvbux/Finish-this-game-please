using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObjectScript : MonoBehaviour
{
    #region Outline Variables/Settings
    [SerializeField] GameManager.OutlineParams outlineParameters;
    Outline outline;
    #endregion

    #region Hidden/Private Variables
    [HideInInspector] public Rigidbody rigidBody => GetComponent<Rigidbody>();
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Awake()
    {
        #region Initialization
        //  Changes the tag to "Grabbable" to prevent bugs.
        gameObject.tag = "Grabbable";
        #endregion
    }

    void Update()
    {
        //  Runs a function to check if the object should be highlighted or not.
        CheckForHighlight();
    }
    #endregion

    #region Custom Functions
    public void Initialize()
    {
        //  Sets the rigidBody's drag to 1 if it is 0, else keep it the same.
        rigidBody.drag = rigidBody.drag == 0 ? 1 : rigidBody.drag;
    }

    public void CheckForHighlight()
    {
        //  Checks if the selected GameObject is this GameObject.
        if (HoverScript.selectedGameObject == gameObject)
        {
            #region Add Outline
            //  If there is not outline, add one.
            if (outline == null)
                outline = GameManager.AddOutline(gameObject, outlineParameters);
            #endregion
        }
        else
        {
            #region Delete Outline
            //  Deletes the outline if it is not selected and there is an outline still active.
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
