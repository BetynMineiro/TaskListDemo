using Microsoft.Extensions.DependencyInjection;
using Moq;
using Task.Application.Task.Update.Dto;
using Task.CrossCutting.Bus;
using Task.Domain.Repositories;

namespace TestProject._2___ApplicationCore.Application.UserCases.Task;

public class UpdateTaskUserCaseTest : TestBase
{
    private IBus _bus;

    private readonly Mock<ITaskRepository> _repository = new Mock<ITaskRepository>();

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(provider => _repository.Object);
    }

    [Fact]
    [Trait("UseCase", nameof(UpdateTaskUserCaseTest))]
    public async System.Threading.Tasks.Task Should_Invoke_UpdateAsync_With_Correct_Data()
    {
        // Arrange
        _bus = ServiceProvider.GetService<IBus>();

        var taskInput = new UpdateTaskInput
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Task 3",
            Owner = "User 1",
            Team = "Operations",
        };

        // Act
        await _bus.SendCommandAsync(taskInput, default);

        // Assert
        _repository.Verify(
            x => x.UpdateAsync(It.Is<string>(id => id == taskInput.Id), 
                It.Is<global::Task.Domain.Entities.Task>(t => t.Name == taskInput.Name 
                                                              && t.Owner == taskInput.Owner 
                                                              && t.Team == taskInput.Team), 
                It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Fact]
    [Trait("UseCase", nameof(UpdateTaskUserCaseTest))]
    public async System.Threading.Tasks.Task Should_Not_Update_Task_When_Name_Is_Null()
    {
        // Arrange
        _bus = ServiceProvider.GetService<IBus>();

        // Act & Assert
        await _bus.SendCommandAsync(new UpdateTaskInput
        {
            Id = Guid.NewGuid().ToString(),
            Name = null,
            Owner = "Test",
            Team = "Development",
        }, default);

        _repository.Verify(
            x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<global::Task.Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    [Trait("UseCase", nameof(UpdateTaskUserCaseTest))]
    public async System.Threading.Tasks.Task Should_Not_Update_Task_When_Owner_Is_Empty()
    {
        // Arrange
        _bus = ServiceProvider.GetService<IBus>();

        // Act & Assert
        await _bus.SendCommandAsync(new UpdateTaskInput
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Task 2",
            Owner = "",
            Team = "Development",
        }, default);
        

        _repository.Verify(
            x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<global::Task.Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never());
    }
    [Fact]
    [Trait("UseCase", nameof(UpdateTaskUserCaseTest))]
    public async System.Threading.Tasks.Task Should_Not_Update_Task_When_Team_Is_Empty()
    {
        // Arrange
        _bus = ServiceProvider.GetService<IBus>();

        // Act & Assert
        await _bus.SendCommandAsync(new UpdateTaskInput
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Task 2",
            Owner = "Test",
            Team = "",
        }, default);
        

        _repository.Verify(
            x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<global::Task.Domain.Entities.Task>(), It.IsAny<CancellationToken>()), Times.Never());
    }

}