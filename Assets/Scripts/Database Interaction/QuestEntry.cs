using SQLite4Unity3d;

public class QuestEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string LocalPath { get; set; }

    public bool Completed { get; set; }
    public bool Active { get; set; }

    public string Name { get; set; }

    [MaxLength(300)]
    public string Description { get; set; }

    // Maybe a problem in the future
    [MaxLength(1000)]
    public string ObjectiveData { get; set; }

    public override string ToString()
    {
        return $"[QuestEntry: Id={Id}, Completed={Completed}, Active={Active}, Name={Name}], ObjectiveData={ObjectiveData}";
    }
}

