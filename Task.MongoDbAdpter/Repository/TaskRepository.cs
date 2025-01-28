using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using Task.CrossCutting.ResultObjects;
using Task.Domain.Messages.Queries.Task;
using Task.Domain.Messages.Queries.Task.Dto;
using Task.Domain.Repositories;
using Task.MongoDbAdpter.Mapper;
using Task.MongoDbAdpter.Repository.Base;

namespace Task.MongoDbAdpter.Repository;

public class TaskRepository : RepositoryBase<Entities.Task>, ITaskRepository
{
    private readonly ILogger _logger;

    public TaskRepository(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, ILogger logger
        ) : base(mongoClient, clientSessionHandle, logger )
    {
        this._logger = logger;
    }

    public async Task<Domain.Entities.Task?> GetAsync(string id, CancellationToken cancellationToken)
    {
        _logger.Information("Retrieving Task entity by ID: {Id}", id);
        try
        {
            var data = await GetByIdAsync(id, cancellationToken);
            _logger.Information("Task entity retrieved successfully by ID: {Id}", id);
            return data.ToDomain();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving Task entity by ID: {Id}", id);
            throw;
        }
    }

    public async Task<string> AddAsync(Domain.Entities.Task task, CancellationToken cancellationToken)
    {
        _logger.Information("Adding Task entity");
        try
        {
            var entity = new Entities.Task()
            {
                Name = task.Name,
                Owner = task.Owner,
                Team = task.Team,
            };
            var result = await InsertAsync(entity, cancellationToken);
            _logger.Information("Task entity added successfully with ID: {Id}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error adding Task entity");
            throw;
        }
    }

    public async Task<string> RemoveAsync(string id, CancellationToken cancellationToken)
    {
        _logger.Information("Removing Task entity by ID: {Id}", id);
        try
        {
            await base.DeleteAsync(id, cancellationToken);
            _logger.Information("Task entity removed successfully by ID: {Id}", id);
            return id;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error removing Task entity by ID: {Id}", id);
            throw;
        }
    }

    public async Task<string> UpdateAsync(string id, Domain.Entities.Task task, CancellationToken cancellationToken)
    {
        _logger.Information("Updating Task entity with ID: {Id}", id);
        try
        {
            var entity = new Entities.Task()
            {
                Id = new ObjectId(id),
                Name = task.Name,
                Owner = task.Owner,
                Team = task.Team,
            };
            await UpdateAsync(id, entity, cancellationToken);
            _logger.Information("Task entity updated successfully with ID: {Id}", id);
            return id;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating Task entity with ID: {Id}", id);
            throw;
        }
    }

    public async Task<Pagination<TaskQueryResult>> Handle(GetFilteredTaskListQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Handling GetFilteredTaskListQuery with FilterValue: {FilterValue}", request.FilterValue);
        try
        {
            var inputList = !string.IsNullOrWhiteSpace(request.FilterValue) ? request.FilterValue.Split(" ") : null;
            var filterList = new List<FilterDefinition<Entities.Task>>();

            if (request.PagedRequest.Status != null)
            {
                filterList.Add(FilterBuilder.Eq(s => s.Status, request.PagedRequest.Status.Value));
            }

            if (inputList != null)
            {
                foreach (var item in inputList)
                {
                    var name = FilterBuilder.Regex(c => c.Name, new BsonRegularExpression(item, "i"));
                    var team = FilterBuilder.Regex(c => c.Team, new BsonRegularExpression(item, "i"));
                    var ownerFilter = FilterBuilder.Regex(c => c.Owner, new BsonRegularExpression(item, "i"));
                    filterList.Add(name);
                    filterList.Add(team);
                    filterList.Add(ownerFilter);
                }
            }

            var filter = filterList.Count != 0 ? FilterBuilder.Or(filterList) : FilterBuilder.Empty;

            var pagedResult = await GetByFilterPagedAsync(filter, request.PagedRequest.PageNumber, request.PagedRequest.PageSize, cancellationToken);

            _logger.Information("Filtered tasks retrieved with total items: {TotalItems}", pagedResult.TotalItems);
            return new Pagination<TaskQueryResult>(pagedResult.Items.ToDomainQueryResultList(), pagedResult.TotalItems, pagedResult.PageNumber, pagedResult.PageSize);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error handling GetFilteredTaskListQuery with FilterValue: {FilterValue}", request.FilterValue);
            throw;
        }
    }

    public async Task<TaskQueryResult> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Information("Handling GetTaskByIdQuery | ID: {Id}", request.Id);
        try
        {
            var data = await this.GetByIdAsync(request.Id, cancellationToken);
            var result = data.ToDomainQueryResult();
            _logger.Information("Handled GetTaskByIdQuery successfully | ID: {Id}", request.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error handling GetTaskByIdQuery | ID: {Id}", request.Id);
            throw;
        }
    }
}