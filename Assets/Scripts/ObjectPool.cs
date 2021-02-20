using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Object to used")]
    public GameObject objPrefab;

    [Header("How many on init?")]
    public int createOnStart;

    private List<GameObject> pooledObjs = new List<GameObject>();

    private void Start()
    {
        for(int  x = 0; x < createOnStart; x++)
        {
            CreateNewObject();
        }
    }

    /**
     * Create a new object and put into the list of pools
     * @return GameObject
     */
    GameObject CreateNewObject()
    {
        // Create the object
        GameObject obj = Instantiate(objPrefab);

        // Disable it right away
        obj.SetActive(false);
        // Add to our allowed list
        pooledObjs.Add(obj);

        return obj;
    }

    /**
     * Get the current game object
     * @return GameObject
     */
    public GameObject GetObject()
    {
        // Find the object from the pool list
        GameObject obj = pooledObjs.Find(x => !x.activeInHierarchy);

        // Create if not found any
        if (!obj)
           obj = CreateNewObject();

        // Be sure is activated, since if was from the list it must be disabled
        obj.SetActive(true);

        return obj;
    }
}
