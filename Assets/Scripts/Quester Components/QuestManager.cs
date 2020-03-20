using System.Collections.Generic;
using NaughtyAttributes;
using Quester.QuestEditor;
using Quester.Utilities;
using SQLite4Unity3d;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuesterAssetMap _assetMap;

    private List<QuestGraph> _activeQuests = new List<QuestGraph>();

    private void Start()
    {
        // allow asset map to generate map for primary keys->graphs
        _assetMap.Initialize();

        // open database
        var dbPath = Application.streamingAssetsPath + "/quests.db";
        var connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // query for active quests
        var questResults = connection.Query<QuestEntry>("select * from QuestEntry where Active = ?", true);

        connection.Close();

        // apply quest objective activity states to retrieved graphs
        foreach (var questResult in questResults)
        {
            if (_assetMap.TryRetrieve(questResult.Id, out QuestGraph graph))
            {
                graph.ApplyCompletionMap(JsonHelper.FromJson<QuestGraph.CompletionMarker>(questResult.ObjectiveData));
                _activeQuests.Add(graph);
            }
            else
            {
                Debug.LogError($"Quest Graph with primary key '{questResult.Id}' could not be retrieved from the Asset Map");
                break;
            }

            // Add objective components to quest objective objects
            foreach (var node in graph.nodes)
            {
                if (node is QuestNode qNode)
                {
                    qNode.InitializeObjective();
                }
            }
        }
    }

    [Button]
    public void Save()
    {
        var dbPath = Application.streamingAssetsPath + "/quests.db";
        var connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        // apply changes
        foreach (var quest in _activeQuests)
        {
            quest.UpdateDatabaseEntry();
        }

        connection.Close();
    }
}