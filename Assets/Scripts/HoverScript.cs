using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float maxReach = 5f;

    public static GameObject selectedGameObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxReach, layerMask))
            selectedGameObject = hit.collider.gameObject;
        else
            selectedGameObject = null;
    }
}
