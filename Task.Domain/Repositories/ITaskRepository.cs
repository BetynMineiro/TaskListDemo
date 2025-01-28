
using MediatR;
using Task.CrossCutting.ResultObjects;
using Task.Domain.Messages.Queries.Task;
using Task.Domain.Messages.Queries.Task.Dto;

namespace Task.Domain.Repositories;

public interface ITaskRepository:
    IRequestHandler<GetFilteredTaskListQuery, Pagination<TaskQueryResult>>,
    IRequestHandler<GetTaskByIdQuery, TaskQueryResult>
{
    Task<Task.Domain.Entities.Task?> GetAsync(string id,  CancellationToken cancellationToken);

    Task<string> AddAsync(Task.Domain.Entities.Task task, CancellationToken cancellationToken);
    Task<string> RemoveAsync(string id, CancellationToken cancellationToken);
    Task<string> UpdateAsync(string id, Task.Domain.Entities.Task task, CancellationToken cancellationToken);

}