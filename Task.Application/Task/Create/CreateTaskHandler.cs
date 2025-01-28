using MediatR;
using Serilog;
using Task.Application.ApplicationServices.NotificationService;
using Task.Application.Task.Create.Dto;
using Task.Domain.Repositories;
using Task.Domain.Validators.Interfaces;

namespace Task.Application.Task.Create;

public class CreateTaskHandler(
    ILogger logger,
    IIsValidTaskValidator validator,
    NotificationServiceContext notificationServiceContext,
    ITaskRepository repository)
    : IRequestHandler<CreateTaskInput, CreateTaskOutput>
{
    public async Task<CreateTaskOutput> Handle(CreateTaskInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting CreateTaskHandler execution.");

        try
        {
            logger.Debug("Initializing task entity creation for Name: {Name}, Owner: {Owner}, Team: {Team}", request.Name, request.Owner, request.Team);
            var entity = new Domain.Entities.Task()
            {
                Name = request.Name,
                Owner = request.Owner,
                Team = request.Team,
            };

            logger.Debug("Validating task entity.");
            var result = await validator.ValidateAsync(entity, cancellationToken);
            if (!result.IsValid)
            {
                logger.Warning("Task validation failed with errors: {Errors}", result.Errors);
                notificationServiceContext.AddNotifications(result);
                return default;
            }

            logger.Debug("Adding task entity to repository.");
            var created = await repository.AddAsync(entity, cancellationToken);
            if (string.IsNullOrWhiteSpace(created))
            {
                logger.Warning("Failed to create task entity.");
                notificationServiceContext.AddNotification("Failed to create task.");
                return default;
            }

            logger.Information("Task successfully created with Id: {TaskId}", created);
            return new CreateTaskOutput() { Id = created };
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An unexpected error occurred during task creation: {ErrorMessage}", ex.Message);
            return default;
        }
    }
}