using MediatR;

namespace Task.Application.Task.Update.Dto;

public class UpdateTaskInput: IRequest<UpdateTaskOutput>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Team { get; set; }

}