﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int pooledAmount;
    public GameObject pooledObject;
    public bool expandable;
    public List<GameObject> pooledObjects;
}

public class ObjectPooler : MonoBehaviour
{
    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("Only one object pooler should be in the scene!");
            Destroy(this);
        }
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    public List<ObjectPoolItem> pooledItems;
    
    private void Start()
    {
        foreach (ObjectPoolItem item in pooledItems)
        {
            for (int i = 0; i < item.pooledAmount; i++)
            {
                GameObject obj = Instantiate(item.pooledObject);
                obj.SetActive(false);
                item.pooledObjects.Add(obj);
            }
        }
    }

    /// <summary>
    /// Get an object from the object pool
    /// </summary>
    /// <param name="prefab">Prefab to get from object pool</param>
    /// <returns>An instance of the pooled object prefab</returns>
    public GameObject GetPooledObject(GameObject prefab)
    {
        foreach (ObjectPoolItem item in pooledItems)
        {
            if (item.pooledObject == prefab)
            {
                foreach (var obj in item.pooledObjects)
                {
                    if (!obj.activeInHierarchy)
                    {
                        return obj;
                    }
                }
                
                // See if we can expand the pool if no objects available
                if (item.expandable)
                {
                    GameObject newObject = Instantiate(item.pooledObject);
                    newObject.SetActive(false);
                    item.pooledObjects.Add(newObject);
                    return newObject;
                }

                return null;
            }
        }

        return null;
    }
}
