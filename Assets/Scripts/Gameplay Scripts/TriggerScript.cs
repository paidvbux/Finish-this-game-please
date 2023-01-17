using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool inTrigger;
    [SerializeField] string[] tagsToCheck = new string[] { "Player" };

    [Header("Object Storage Settings")]
    public bool storeObjects;
    public Dictionary<GameObject, string> storedObjects;

    int numObjectsInTrigger;

    void Awake()
    {
        storedObjects = new Dictionary<GameObject, string>();
    }

    void Update()
    {
        inTrigger = numObjectsInTrigger > 0;
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (string tagToCheck in tagsToCheck) 
        { 
            if (other.gameObject.CompareTag(tagToCheck) && !other.isTrigger)
            {
                if (storeObjects)
                    storedObjects.Add(other.gameObject, other.tag);
                numObjectsInTrigger++;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        foreach (string tagToCheck in tagsToCheck)
        {
            if (other.gameObject.CompareTag(tagToCheck))
            {
                if (storeObjects)
                    storedObjects.Remove(other.gameObject);
                numObjectsInTrigger--;
            }
        }
    }
}
