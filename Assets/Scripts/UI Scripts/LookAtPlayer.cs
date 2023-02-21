using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    Canvas canvas => GetComponent<Canvas>();

    void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        Vector3 position = new Vector3(GameManager.Player.position.x, transform.position.y, GameManager.Player.position.z);

        transform.LookAt(position);
        transform.localEulerAngles += offset;
    }
}
