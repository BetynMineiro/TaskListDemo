using Task.CrossCutting.Enum.Common;

namespace Task.Domain.Messages.Queries.Task.Dto;

public class TaskQueryResult
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Team { get; set; }
    public Status Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}