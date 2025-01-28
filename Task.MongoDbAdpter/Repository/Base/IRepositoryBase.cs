
using MongoDB.Driver;
using Task.CrossCutting.ResultObjects;
using Task.MongoDbAdpter.Entities.Base;

namespace Task.MongoDbAdpter.Repository.Base;

public interface IRepositoryBase<T> where T : MongoBaseEntity
{
    Task<string> InsertAsync(T entity, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(string id,CancellationToken cancellationToken);
    Task<T> GetSingleAsync(FilterDefinition<T> filter,CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<Pagination<T>> GetAllPagedAsync(int pageNumber, int pageSize,CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetByFilterAsync(FilterDefinition<T> filter,CancellationToken cancellationToken);
    Task<Pagination<T>> GetByFilterPagedAsync(FilterDefinition<T> filter, int pageNumber, int pageSize,CancellationToken cancellationToken);
    System.Threading.Tasks.Task UpdateAsync(string id, T entity,CancellationToken cancellationToken);
    System.Threading.Tasks.Task DeleteAsync(string id,CancellationToken cancellationToken);
}