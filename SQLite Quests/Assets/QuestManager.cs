using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<Quest> _loadedQuests = new List<Quest>();
    public Stage stage;

    private SQLiteConnection _connection;

    private void Awake()
    {
#if UNITY_EDITOR
        var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        var questsResults = _connection.Table<QuestEntry>();

        foreach (var questResult in questsResults)
        {
            _loadedQuests.Add(JsonUtility.FromJson<Quest>(questResult.Data));
        }

        _connection.Close();

        Debug.Log($"Loaded {_loadedQuests.Count} quests from database.");

        foreach (var quest in _loadedQuests)
        {
            foreach (var objective in quest.Objectives)
            {
                var interactor = Objective.AddQuestInteractor(stage.actors[objective.InteractWith].gameObject, objective.Type);
                interactor.Initialize(this, quest, objective);
            }
        }
    }
}
