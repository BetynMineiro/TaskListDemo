using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Task.Application.Task.Create.Dto;
using Task.Application.Task.Delete.Dto;
using Task.Application.Task.Update.Dto;
using Task.CrossCutting.Bus;
using Task.CrossCutting.RequestObjects;
using Task.CrossCutting.ResultObjects;
using Task.Domain.Messages.Queries.Task;
using Task.Domain.Messages.Queries.Task.Dto;

namespace Task.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TaskController(IBus bus) : ControllerBase
{
    [HttpGet()]
    [ProducesResponseType(typeof(Result<Pagination<TaskQueryResult>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetTaskList( [FromQuery] PagedRequest pagedRequest, [FromQuery] string? filter,  CancellationToken cancellationToken)
    {
        var result = await bus.SendQueryAsync(
            new GetFilteredTaskListQuery(filter, pagedRequest), cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<TaskQueryResult>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetTask([Required] string id,
        CancellationToken cancellationToken)
    {
        var result = await bus.SendQueryAsync(new GetTaskByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<CreateTaskOutput>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> PostTask([FromBody] CreateTaskInput input,
        CancellationToken cancellationToken)
    {
        var result = await bus.SendCommandAsync(input, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<UpdateTaskOutput>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> PutTask([Required] string id,
        [FromBody] UpdateTaskInput input,
        CancellationToken cancellationToken)
    {
        input.Id = id;
        var result = await bus.SendCommandAsync(input, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<DeleteTaskOutput>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteTask([Required] string id,
        CancellationToken cancellationToken)
    {
        var result = await bus.SendCommandAsync(new DeleteTaskInput() { Id = id }, cancellationToken);
        return Ok(result);
    }
}