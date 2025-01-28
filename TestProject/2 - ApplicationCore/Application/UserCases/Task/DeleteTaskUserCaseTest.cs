using Microsoft.Extensions.DependencyInjection;
using Moq;
using Task.Application.Task.Delete.Dto;
using Task.CrossCutting.Bus;
using Task.Domain.Repositories;

namespace TestProject._2___ApplicationCore.Application.UserCases.Task;

public class DeleteTaskUserCaseTest : TestBase
{
    private IBus _bus;

    private readonly Mock<ITaskRepository> _repository = new Mock<ITaskRepository>();

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(provider => _repository.Object);
    }

    [Fact]
    [Trait("UseCase", nameof(DeleteTaskUserCaseTest))]
    public async System.Threading.Tasks.Task Should_Invoke_DeleteAsync_With_Correct_Id()
    {
        // Arrange
        _bus = ServiceProvider.GetService<IBus>();

        var taskId = Guid.NewGuid().ToString();

        // Act
        await _bus.SendCommandAsync(new DeleteTaskInput() { Id = taskId }, default);

        // Assert
        _repository.Verify(
            x => x.RemoveAsync(It.Is<string>(id => id == taskId), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    [Trait("UseCase", nameof(DeleteTaskUserCaseTest))]
    public async System.Threading.Tasks.Task Should_Not_Delete_Task_When_Id_Is_Empty()
    {
        // Arrange
        _bus = ServiceProvider.GetService<IBus>();

        // Act & Assert

        await _bus.SendCommandAsync(new DeleteTaskInput(){Id = String.Empty} , default);

        _repository.Verify(
            x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}