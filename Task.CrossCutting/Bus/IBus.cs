namespace Task.CrossCutting.Bus;

public interface IBus
{
    Task<object> SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken);
    System.Threading.Tasks.Task SendEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
    Task<object> SendQueryAsync<TQuery>(TQuery query, CancellationToken cancellationToken);
}