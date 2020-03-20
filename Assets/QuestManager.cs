using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Quester.QuestEditor;
using Quester.Utilities;
using SQLite4Unity3d;
using UnityEditor;
using UnityEngine;

public interface IQuestVisitor
{
    void Visit(TalkToObjective objective);
    //...
}

public class QuestManager : MonoBehaviour, IQuestVisitor
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

            foreach (var node in graph.nodes)
            {
                if (node is QuestNode qNode)
                {
                    // map id to gameObject and add objective component, setting it active if objective is active
                    switch (qNode)
                    {
                        case ActivateNode activateNode:
                            var interactable = ObjectIDMap.Instance.Get(activateNode.Interactable);
                            break;
                        case CollectNode collectNode:
                            var collectable = ObjectIDMap.Instance.Get(collectNode.Item);
                            break;
                        case DeliverNode deliverNode:
                            var deliverable = ObjectIDMap.Instance.Get(deliverNode.Deliverable);
                            var receiver = ObjectIDMap.Instance.Get(deliverNode.Receiver);
                            break;
                        case KillNode killNode:
                            var target = ObjectIDMap.Instance.Get(killNode.Target);
                            break;
                        case TalkToNode talkToNode:
                        {
                            var speakerGameObject = ObjectIDMap.Instance.Get(talkToNode.Speaker);
                            var objective = speakerGameObject.AddComponent<TalkToObjective>();
                            objective.SetNode(talkToNode);
                        }
                            var speaker = ObjectIDMap.Instance.Get(talkToNode.Speaker);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(qNode));
                    }
                }
            }
        }

        //Debug.Log($"Loaded {_loadedQuests.Count} quests from database.");

        //foreach (var quest in _loadedQuests)
        //{
        //    foreach (var objective in quest.Objectives)
        //    {
        //        var interactor = Objective.AddQuestInteractor(stage.actors[objective.InteractWith].gameObject, objective.Type);
        //        interactor.Initialize(this, quest, objective);
        //    }
        //}
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


    public void Visit(/*ref*/ TalkToObjective objective)
    {
        //objective.Speaker = ;
    }
}

//public class QuestManager : MonoBehaviour
//{
//    private List<Quest> _loadedQuests = new List<Quest>();
//    public Stage stage;

//    private SQLiteConnection _connection;

//    private void Awake()
//    {
//#if UNITY_EDITOR
//        var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
//#endif
//        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
//        var questsResults = _connection.Table<QuestEntry>();

//        foreach (var questResult in questsResults)
//        {
//            _loadedQuests.Add(JsonUtility.FromJson<Quest>(questResult.ObjectiveData));
//        }

//        _connection.Close();

//        Debug.Log($"Loaded {_loadedQuests.Count} quests from database.");

//        foreach (var quest in _loadedQuests)
//        {
//            foreach (var objective in quest.Objectives)
//            {
//                var interactor = Objective.AddQuestInteractor(stage.actors[objective.InteractWith].gameObject, objective.Type);
//                interactor.Initialize(this, quest, objective);
//            }
//        }
//    }
//}
