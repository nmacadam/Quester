using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEditor;
using UnityEngine;

public class QuestEditor : EditorWindow
{
    //private static List<Quest> _quests;

    private static SQLiteConnection _connection;
    private static TableQuery<QuestEntry> quests;

    [MenuItem("Window/Quest Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(QuestEditor));

        var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        quests = _connection.Table<QuestEntry>();
    }

    void OnGUI()
    {
        //if (GUILayout.Button("Refresh"))
        //{
        //    _quests = FindAssetsByType<Quest>();
        //}

        foreach (var questEntry in quests)
        {
            GUILayout.BeginHorizontal("Box");

            if (GUILayout.Button("Remove from DB", GUILayout.Width(150)))
            {
                _connection.Delete(questEntry);
                quests = _connection.Table<QuestEntry>();
            }

            GUILayout.Label($"PK: {questEntry.Id}", GUILayout.Width(50));
            GUILayout.Label($"Name: {questEntry.Name}");

            GUILayout.EndHorizontal();
        }

    }

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}
