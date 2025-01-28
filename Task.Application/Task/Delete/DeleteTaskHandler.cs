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
        logger.Information("DeleteTaskHandler - Start processing task deletion with ID: {TaskId}", request.Id);

        try
        {
            var deleted = await taskRepository.RemoveAsync(request.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(deleted))
            {
                logger.Warning("DeleteTaskHandler - Task deletion failed. Task ID: {TaskId}", request.Id);
                notificationServiceContext.AddNotification($"Delete Task {request.Id} failed");
                return default;
            }

            logger.Information("DeleteTaskHandler - Task successfully deleted. Task ID: {TaskId}", request.Id);
            return new DeleteTaskOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "DeleteTaskHandler - An exception occurred during task deletion. Task ID: {TaskId}, Error: {ErrorMessage}", request.Id, ex.Message);
            return default;
        }
    }
}