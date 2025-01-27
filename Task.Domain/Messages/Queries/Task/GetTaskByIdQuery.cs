using MediatR;
using Task.Domain.Messages.Queries.Task.Dto;

namespace Task.Domain.Messages.Queries.Task;

public class GetTaskByIdQuery(string id) : IRequest<TaskQueryResult>
{
    public string Id { get; } = id;
}