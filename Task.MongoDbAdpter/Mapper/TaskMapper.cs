using Task.Domain.Messages.Queries.Task.Dto;

namespace Task.MongoDbAdpter.Mapper;

public static class TaskMapper
{
    public static Domain.Entities.Task ToDomain(this Entities.Task? task)
    {
        if (task is null)
            return default!;

        return new Domain.Entities.Task
        {
            Id = task.Id.ToString(),
            Name = task.Name,
            Owner = task.Owner,
            Team = task.Team,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
        };
    }

    public static TaskQueryResult ToDomainQueryResult(this Entities.Task? task)
    {
        if (task is null)
            return default!;

        return new TaskQueryResult
        {
            Id = task.Id.ToString(),
            Name = task.Name,
            Owner = task.Owner,
            Team = task.Team,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
        };
    }

    public static IEnumerable<TaskQueryResult> ToDomainQueryResultList(
        this IEnumerable<Entities.Task> tasks)
    {
        if (!tasks.Any())
        {
            return default;
        }

        return tasks.Select(clinic => clinic.ToDomainQueryResult());
    }
}