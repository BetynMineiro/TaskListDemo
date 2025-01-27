using MediatR;
using Serilog;

namespace Task.CrossCutting.Bus;

public class Bus(IMediator mediator, ILogger logger) : IBus
{
    public async Task<object> SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
    {
        logger.Information("Calling Command Bus | Class: DNTL.Task.CrossCutting.Bus | Method: SendCommandAsync");

        return await mediator.Send(command, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task SendEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        logger.Information("Calling Event Bus | Class: DNTL.Task.CrossCutting.Bus | Method: SendEventAsync");

        await mediator.Publish(@event, cancellationToken).ConfigureAwait(false);
    }

    public async Task<object> SendQueryAsync<TQuery>(TQuery query, CancellationToken cancellationToken)
    {
        logger.Information("Calling Query Bus | Class: DNTL.Task.CrossCutting.Bus | Method: SendQueryAsync");

        return await mediator.Send(query, cancellationToken).ConfigureAwait(false);
    }
}