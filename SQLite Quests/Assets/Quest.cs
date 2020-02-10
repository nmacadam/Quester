using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


[System.Serializable]
public class Interaction
{
    public string thing;
}

[System.Serializable]
public class QuestInteraction
{
    // todo: later extend to InteractableObject or something more generic than an actor
    public ActorID InteractWith;
    //public TalkToObjective ;
}

// note: don't modify quest inside quest data at runtime.  it's a definition! it will turn the wrapped quest
// into JSON to go in the database

[System.Serializable]
public class Quest
{
    private static int _pkIncrementor;
    [ShowNonSerializedField] private int _dbID = ++_pkIncrementor;
    public int ID => _dbID;

    public string Name;
    public string Description;
    public List<Objective> Objectives;

    public bool Active;
    public bool Completed;

    public void CompleteObjective(Objective objective)
    {
        objective.Completed = true;
    }
}

[System.Serializable]
public class Objective
{
    public enum ObjectiveType
    {
        TalkTo, Kill, Collect, Deliver, Activate
    }

    // public int ID;
    public ObjectiveType Type;
    public ActorID InteractWith;

    public bool Completed = false;

    public static QuestInteractor AddQuestInteractor(GameObject gameObject, ObjectiveType type)
    {
        switch (type)
        {
            case ObjectiveType.TalkTo:
                return gameObject.AddComponent<QuestTalkToInterator>();
            //case ObjectiveType.Kill:
            //    break;
            //case ObjectiveType.Collect:
            //    break;
            //case ObjectiveType.Deliver:
            //    break;
            //case ObjectiveType.Activate:
            //    break;
            default:
                return gameObject.AddComponent<QuestInteractor>();
        }
    }
}



//[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest")]
//public class Quest : ScriptableObject
//{
//    //private static int _pkIncrementor;
//   // [ShowNonSerializedField] private int _dbID = ++_pkIncrementor;

//    public string Name;
//    public string Description;

//    // Reward

//    // Location

//    // Type? Side Quest?

//    // Branching?

//    [SerializeField]/*[ReorderableList]*/ private List<Objective> _objectives;

//    //[Button]
//    //public void CreateDatabase()
//    //{
//    //    var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
//    //    SQLiteConnection connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

//    //    connection.DropTable<QuestEntry>();
//    //    connection.CreateTable<QuestEntry>();

//    //    connection.Close();
//    //}

//    //[Button]
//    //public void UpdateDatabaseEntry()
//    //{
//    //    var dbPath = $@"Assets/StreamingAssets/{"quests.db"}";
//    //    SQLiteConnection connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

//    //    int pk = connection.InsertOrReplace(
//    //        new QuestEntry
//    //        {
//    //            Id = _dbID,
//    //            Active = true,
//    //            Completed = false,
//    //            Name = this.Name,
//    //            Data = JsonUtility.ToJson(this)
//    //        }
//    //    );

//    //    //if (_dbID == -1)
//    //    //{
//    //    //    _dbID = connection.Insert(new QuestEntry
//    //    //    {
//    //    //        Active = false,
//    //    //        Completed = false,
//    //    //        Name = this.Name,
//    //    //        Data = JsonUtility.ToJson(this)
//    //    //    });
//    //    //}
//    //    //else
//    //    //{
//    //    //    var result = connection.Table<QuestEntry>().Where(x => x.Id == _dbID);

//    //    //    foreach (QuestEntry questEntry in result)
//    //    //    {
//    //    //        questEntry.Name = this.Name;
//    //    //        questEntry.Data = JsonUtility.ToJson(this);
//    //    //    }
//    //    //}

//    //    //connection.Close();
//    //}
//}

//[System.Serializable]
//public class Objective
//{
//    public enum ObjectiveType
//    {
//        Kill, Collect, Deliver, Activate
//    }

//    public ObjectiveType Type;
//    public bool Completed = false;
//}