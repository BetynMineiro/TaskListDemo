using MediatR;
using Task.CrossCutting.RequestObjects;
using Task.CrossCutting.ResultObjects;
using Task.Domain.Messages.Queries.Task.Dto;

namespace Task.Domain.Messages.Queries.Task;

public class GetFilteredTaskListQuery(string? filterValue, PagedRequest pagedRequest)
    : IRequest<Pagination<TaskQueryResult>>
{
    public string? FilterValue { get; set; } = filterValue;
    public PagedRequest PagedRequest { get; set; } = pagedRequest;
}