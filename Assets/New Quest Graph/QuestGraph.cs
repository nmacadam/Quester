using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Quester.Utilities;
using SQLite4Unity3d;
using UnityEditor;
using UnityEngine;
using XNode;

namespace Quester.QuestEditor
{
    [CreateAssetMenu]
    public class QuestGraph : NodeGraph 
    {
        // Internal graph code



        // Quest data and database interface

        [Header("Quest Data")] 
        [SerializeField] private int _primaryKey = -1;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private bool _active;
        [SerializeField] private bool _complete;

        public int PrimaryKey => _primaryKey;

        public void UpdateDatabaseEntry()
        {
            var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
            SQLiteConnection connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            //connection.Query<QuestEntry>("select * from QuestEntry where Id = ")

            if (_primaryKey == -1)
            {
                _primaryKey = connection.Insert(
                    new QuestEntry
                    {
                        //Id = Quest.ID,
                        LocalPath = AssetDatabase.GetAssetPath(this),
                        Active = _active,
                        Completed = _complete,
                        Name = _name,
                        Description = _description,
                        ObjectiveData = JsonHelper.ToJson(GetCompletionMap())
                    },
                    "",
                    typeof(QuestEntry)
                );
            }
            else
            {
                connection.InsertOrReplace(
                    new QuestEntry
                    {
                        Id = _primaryKey,
                        LocalPath = AssetDatabase.GetAssetPath(this),
                        Active = _active,
                        Completed = _complete,
                        Name = _name,
                        Description = _description,
                        ObjectiveData = JsonHelper.ToJson(GetCompletionMap())
                    },
                    typeof(QuestEntry)
                );
            }
            
            connection.Close();

            Debug.Log($"Quest '{_name}' was successfully updated.");
        }

        [System.Serializable]
        public struct CompletionMarker
        {
            [SerializeField] public int NodeIterator;
            [SerializeField] public bool IsComplete;

            public CompletionMarker(int nodeIterator, bool isComplete)
            {
                NodeIterator = nodeIterator;
                IsComplete = isComplete;
            }
        }

        public CompletionMarker[] GetCompletionMap()
        {
            var stateMap = new List<CompletionMarker>();

            for (var i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is QuestNode questNode)
                {
                    stateMap.Add(new CompletionMarker(i, questNode.complete));
                }
            }

            return stateMap.ToArray();
        }

        public void ApplyCompletionMap(CompletionMarker[] map)
        {
            foreach (var state in map)
            {
                ((QuestNode) nodes[state.NodeIterator]).complete = state.IsComplete;
            }
        }
    }
}