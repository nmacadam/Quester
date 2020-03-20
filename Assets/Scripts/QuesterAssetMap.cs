using System.Collections;
using System.Collections.Generic;
using Quester.QuestEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Asset Map", menuName = "Asset Map")]
public class QuesterAssetMap : ScriptableObject
{
    [SerializeField] private List<QuestGraph> _quests;
    private static Dictionary<int, QuestGraph> _questMap = new Dictionary<int, QuestGraph>();

    private static bool _hasInitialized = false;

    public void Initialize()
    {
        foreach (var quest in _quests)
        {
            Register(quest);
        }
    }

    private void Register(QuestGraph graph)
    {
        _questMap[graph.PrimaryKey] = graph;
    }

    //public QuestGraph Retrieve(int primaryKey)
    //{

    //    if (_questMap.ContainsKey(primaryKey))
    //    {
    //        return _questMap[primaryKey];
    //    }

    //    Debug.LogError("Quest Asset Map does not contain primary key " + primaryKey);
    //    return null;
    //}

    public bool TryRetrieve(int primaryKey, out QuestGraph graph)
    {
        graph = null;

        if (_questMap.ContainsKey(primaryKey))
        {
            graph = _questMap[primaryKey];
            return true;
        }

        return false;
    }
}
