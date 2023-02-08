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
        CheckForHighlight();
    }
    #endregion

    #region Custom Functions
    public void Initialize()
    {
        rigidBody.drag = (rigidBody.drag == 0 ? 1 : rigidBody.drag);
    }

    public void CheckForHighlight()
    {
        bool hoveringOver = HoverScript.selectedGameObject == gameObject && GrabScript.singleton.heldRigidbody == null;
        if (hoveringOver || GrabScript.singleton.heldRigidbody == rigidBody)
        {
            #region Add Outline
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
