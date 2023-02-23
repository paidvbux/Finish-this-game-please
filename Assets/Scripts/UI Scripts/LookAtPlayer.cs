using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] bool includesYPosition;

    Canvas canvas => GetComponent<Canvas>();

    void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 position = new Vector3(GameManager.singleton.playerCamera.position.x, includesYPosition ? 
            GameManager.singleton.playerCamera.position.y : 0 + transform.position.y, GameManager.singleton.playerCamera.position.z);

        transform.LookAt(position, Vector3.up);
    }
}
