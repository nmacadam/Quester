using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SQLite4Unity3d;
using UnityEngine;

public class DBAccessor : MonoBehaviour
{
    private SQLiteConnection _connection;

    private void Start()
    {
#if UNITY_EDITOR
        var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

        //_connection.DropTable<QuestEntry>();
        _connection.CreateTable<QuestEntry>();

        //_connection.Insert(
        //    new QuestEntry
        //    {
        //        Active = true,
        //        Completed = false,
        //        Name = "QuestEntry 1"
        //    }
        //);

        var quests = _connection.Table<QuestEntry>();

        foreach (var quest in quests)
        {
            Debug.Log(quest);
        }
    }

    [Button]
    public void ResetDatabase()
    {
        var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
        SQLiteConnection connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        connection.DropTable<QuestEntry>();
        connection.CreateTable<QuestEntry>();

        connection.Close();
    }
}
