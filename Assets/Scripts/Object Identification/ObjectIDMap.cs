using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectIDMap : MonoBehaviour
{
    #region Singleton Functionality

    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static ObjectIDMap m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static ObjectIDMap Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(ObjectIDMap) +
                                 "' already destroyed. Returning null.");
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (ObjectIDMap)FindObjectOfType(typeof(ObjectIDMap));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<ObjectIDMap>();
                        singletonObject.name = typeof(ObjectIDMap).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }

    #endregion

    private Dictionary<Guid, GameObject> _objectMap = new Dictionary<Guid, GameObject>();

    private void Awake()
    {
        //SceneManager.activeSceneChanged += delegate { _objectMap.Clear(); };
    }

    public GameObject Get(ObjectID id)
    {
        return _objectMap[id.GUID];
    }

    public void Add(ObjectID id, GameObject gameObject)
    {
        //_objectMap[id.GUID] = gameObject;
        _objectMap.Add(id.GUID, gameObject);
    }

    public void Remove(ObjectID id)
    {
        _objectMap.Remove(id.GUID);
    }
}