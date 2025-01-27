using MediatR;

namespace Task.Application.Task.Create.Dto;

public class CreateTaskInput : IRequest<CreateTaskOutput>
{
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Team { get; set; }

}