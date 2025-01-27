using Task.Domain.Entities.Base;

namespace Task.Domain.Entities;

public class Task : EntityBase
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Team { get; set; }
}