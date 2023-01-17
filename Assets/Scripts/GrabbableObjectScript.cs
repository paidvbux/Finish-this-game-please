using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObjectScript : MonoBehaviour
{
    [SerializeField] GameManager.OutlineParams outlineParameters;
    Outline outline;

    [HideInInspector] public Rigidbody rigidBody => GetComponent<Rigidbody>();

    public void Initialize()
    {
        rigidBody.drag = rigidBody.drag == 0 ? 1 : rigidBody.drag;
    }

    void Awake()
    {
        gameObject.tag = "Grabbable";
    }

    void Update()
    {
        CheckForHighlight();
    }

    public void CheckForHighlight()
    {
        if (HoverScript.selectedGameObject == gameObject)
        {
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
