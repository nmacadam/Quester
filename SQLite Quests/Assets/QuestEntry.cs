using SQLite4Unity3d;

public class QuestEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public bool Completed { get; set; }
    public bool Active { get; set; }

    public string Name { get; set; }

    public string Data { get; set; }

    public override string ToString()
    {
        return $"[QuestEntry: Id={Id}, Completed={Completed}, Active={Active}, Name={Name}], Data={Data}";
    }
}

