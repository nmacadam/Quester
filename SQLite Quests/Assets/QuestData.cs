using NaughtyAttributes;
using SQLite4Unity3d;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest")]
public class QuestData : ScriptableObject
{
    public Quest Quest;

    [Button]
    public void UpdateDatabaseEntry()
    {
        var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
        SQLiteConnection connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        connection.InsertOrReplace(
            new QuestEntry
            {
                Id = Quest.ID,
                Active = Quest.Active,
                Completed = Quest.Completed,
                Name = Quest.Name,
                Data = JsonUtility.ToJson(Quest)
            }
        );
        connection.Close();

        Debug.Log($"Quest '{Quest.Name}' was successfully updated.");
    }
}