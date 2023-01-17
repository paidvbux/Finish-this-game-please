using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool inTrigger;
    [SerializeField] string[] tagsToCheck = new string[] { "Player" };

    [Header("Object Storage Settings")]
    public bool storeObjects;
    public Dictionary<string, GameObject> storedObjects;

    void OnTriggerEnter(Collider other)
    {
        foreach (string tagToCheck in tagsToCheck) 
        { 
            if (other.gameObject.CompareTag(tagToCheck))
            {
                if (storeObjects)
                    storedObjects.Add(other.tag, other.gameObject);
                inTrigger = true;
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
                    storedObjects.Remove(other.tag);
                inTrigger = false;
            }
        }
    }
}
