using Task.MongoDbAdpter.Entities.Base;

namespace Task.MongoDbAdpter.Entities;

public class Task: MongoBaseEntity
{
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Team { get; set; }
}