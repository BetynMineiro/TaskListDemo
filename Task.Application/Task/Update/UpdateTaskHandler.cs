using MediatR;
using Serilog;
using Task.Application.ApplicationServices.NotificationService;
using Task.Application.Task.Update.Dto;
using Task.Domain.Repositories;
using Task.Domain.Validators.Interfaces;

namespace Task.Application.Task.Update;

public class UpdateTaskHandler(
    ILogger logger,
    IIsValidTaskValidator updateTaskValidator,
    NotificationServiceContext notificationServiceContext,
    ITaskRepository taskRepository)
    : IRequestHandler<UpdateTaskInput, UpdateTaskOutput>
{
    public async Task<UpdateTaskOutput> Handle(UpdateTaskInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting task update | Method: {method} | Class: {class} | TaskId: {TaskId}", nameof(Handle),
            nameof(UpdateTaskHandler), request.Id);

        try
        {
            var entity = new Domain.Entities.Task()
            {
                Id = request.Id,
                Name = request.Name,
                Owner = request.Owner,
                Team = request.Team,
            };

            var result = await updateTaskValidator.ValidateAsync(entity, cancellationToken);
            if (!result.IsValid)
            {
                logger.Warning("Validation failed | TaskId: {TaskId} | Errors: {Errors}", request.Id, result.Errors);
                notificationServiceContext.AddNotifications(result);
                return default;
            }

            logger.Debug("Validation successful | TaskId: {TaskId}", request.Id);

            var updated = await taskRepository.UpdateAsync(entity.Id, entity, cancellationToken);
            if (string.IsNullOrWhiteSpace(updated))
            {
                logger.Warning("Task update failed | TaskId: {TaskId}", request.Id);
                notificationServiceContext.AddNotification($"Task update failed for ID {request.Id}");
                return default;
            }

            logger.Information("Task successfully updated | TaskId: {TaskId} | Method: {method} | Class: {class}", request.Id, nameof(Handle), nameof(UpdateTaskHandler));
            return new UpdateTaskOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An exception occurred during task update | TaskId: {TaskId} | Error: {error}", request.Id, ex.Message);
            return default;
        }
    }
}