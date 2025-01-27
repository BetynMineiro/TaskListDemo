using MediatR;

namespace Task.Application.Task.Delete.Dto;

public class DeleteTaskInput: IRequest<DeleteTaskOutput>
{
    public string Id { get; set; }
}