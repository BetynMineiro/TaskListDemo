using MediatR;
using Serilog;
using Task.Application.ApplicationServices.NotificationService;
using Task.Application.Task.Delete.Dto;
using Task.Domain.Repositories;

namespace Task.Application.Task.Delete;

public class DeleteTaskHandler(
    ILogger logger,
    NotificationServiceContext notificationServiceContext,
    ITaskRepository taskRepository)
    : IRequestHandler<DeleteTaskInput, DeleteTaskOutput>
{
    public async Task<DeleteTaskOutput> Handle(DeleteTaskInput request, CancellationToken cancellationToken)
    {
        logger.Information("DeleteTaskHandler - Start deleting Task with ID: {TaskId}", request.Id);

        try
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                logger.Warning("DeleteTaskHandler - Failed to delete Task with ID: {TaskId}", request.Id);
                notificationServiceContext.AddNotification($"Delete Task {request.Id} failed");
                return default;
            }

            var deleted = await taskRepository.RemoveAsync(request.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(deleted))
            {
                logger.Warning("DeleteTaskHandler - Failed to delete Task with ID: {TaskId} not found", request.Id);
                notificationServiceContext.AddNotification($"Delete Task {request.Id} not found");
                return default;
            }

            logger.Information("DeleteTaskHandler - Successfully deleted Task with ID: {TaskId}", request.Id);
            return new DeleteTaskOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "DeleteTaskHandler - Exception occurred while deleting Task with ID: {TaskId}. Error: {ErrorMessage}", request.Id, ex.Message);
            return default;
        }
    }
}