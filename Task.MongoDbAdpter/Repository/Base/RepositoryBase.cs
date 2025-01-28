using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using Task.CrossCutting.ResultObjects;
using Task.MongoDbAdpter.Context;
using Task.MongoDbAdpter.Entities.Base;

namespace Task.MongoDbAdpter.Repository.Base
{
    public class RepositoryBase<T> : MongoDbContext, IRepositoryBase<T> where T : MongoBaseEntity
    {
        private readonly IMongoClient _mongoClient;
        private readonly IClientSessionHandle _clientSessionHandle;

        private readonly string _collection;
        protected readonly FilterDefinitionBuilder<T> FilterBuilder;
        protected readonly ILogger Logger;

        protected virtual IMongoCollection<T> Collection =>
            _mongoClient.GetDatabase(Database).GetCollection<T>(_collection);

        protected RepositoryBase(IMongoClient mongoClient, IClientSessionHandle clientSessionHandle, ILogger logger)
        {
            var collection = typeof(T).Name.ToLower();
            _mongoClient = mongoClient;
            _clientSessionHandle = clientSessionHandle;
            _collection = collection;
            Logger = logger;
            if (!_mongoClient.GetDatabase(Database).ListCollectionNames().ToList().Contains(collection))
                _mongoClient.GetDatabase(Database).CreateCollection(collection);

            FilterBuilder = Builders<T>.Filter;


        }

        private async System.Threading.Tasks.Task CreateIndexAsync(IndexKeysDefinition<T> indexKeys, CreateIndexOptions options,
            CancellationToken cancellationToken)
        {
            Logger.Information("Creating index on collection {collection}", _collection);
            var indexModel = new CreateIndexModel<T>(indexKeys, options);
            await Collection.Indexes.CreateOneAsync(_clientSessionHandle, indexModel,
                cancellationToken: cancellationToken);
        }

        public async Task<string> InsertAsync(T entity, CancellationToken cancellationToken)
        {
            Logger.Information("Inserting entity into collection {collection}", _collection);
            try
            { 
                entity.SetCreationInfo();
                await Collection.InsertOneAsync(_clientSessionHandle, entity, cancellationToken: cancellationToken);
                Logger.Information("Entity inserted successfully into collection {collection} with ID: {id}", _collection, entity.Id);
                return entity.Id.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error inserting entity into collection {collection}", _collection);
                throw;
            }
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            Logger.Information("Retrieving entity by ID: {id} from collection: {collection}", id, _collection);
            try
            {
                var objectId = new ObjectId(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var result = await Collection.Find(_clientSessionHandle, filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                Logger.Information("Entity retrieved successfully by ID: {id} from collection: {collection}", id, _collection);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error retrieving entity by ID: {id} from collection: {collection}", id, _collection);
                throw;
            }
        }

        public async Task<T> GetSingleAsync(FilterDefinition<T> filter, CancellationToken cancellationToken)
        {
            Logger.Information("Retrieving single entity from collection: {collection}", _collection);
            try
            {
                var result = await Collection.Find(_clientSessionHandle, filter)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                Logger.Information("Single entity retrieved successfully from collection: {collection}", _collection);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error retrieving single entity from collection: {collection}", _collection);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            Logger.Information("Retrieving all entities from collection: {collection}", _collection);
            try
            {
                var result = await Collection.Find(_clientSessionHandle, FilterDefinition<T>.Empty)
                    .ToListAsync(cancellationToken: cancellationToken);
                Logger.Information("All entities retrieved successfully from collection: {collection}", _collection);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error retrieving all entities from collection: {collection}", _collection);
                throw;
            }
        }

        public async Task<Pagination<T>> GetAllPagedAsync(int pageNumber, int pageSize,
            CancellationToken cancellationToken)
        {
            Logger.Information("Retrieving paged entities from collection: {collection} | PageNumber: {pageNumber} | PageSize: {pageSize}", _collection, pageNumber, pageSize);
            try
            {
                var totalItems = await Collection.CountDocumentsAsync(_clientSessionHandle, FilterDefinition<T>.Empty,
                    cancellationToken: cancellationToken);
                var items = await Collection.Find(_clientSessionHandle, FilterDefinition<T>.Empty)
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .ToListAsync(cancellationToken: cancellationToken);
                Logger.Information("Paged entities retrieved successfully from collection: {collection} | PageNumber: {pageNumber} | PageSize: {pageSize}", _collection,
                    pageNumber, pageSize);
                return new Pagination<T>(items, (int)totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error retrieving paged entities from collection: {collection} | PageNumber: {pageNumber} | PageSize: {pageSize}", _collection, pageNumber,
                    pageSize);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetByFilterAsync(FilterDefinition<T> filter,
            CancellationToken cancellationToken)
        {
            Logger.Information("Retrieving entities by filter from collection: {collection}", _collection);
            try
            {
                var result = await Collection.Find(_clientSessionHandle, filter)
                    .ToListAsync(cancellationToken: cancellationToken);
                Logger.Information("Entities retrieved by filter successfully from collection: {collection}", _collection);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error retrieving entities by filter from collection: {collection}", _collection);
                throw;
            }
        }

        public async Task<Pagination<T>> GetByFilterPagedAsync(FilterDefinition<T> filter, int pageNumber,
            int pageSize, CancellationToken cancellationToken)
        {
            Logger.Information("Retrieving paged entities by filter from collection: {collection} | PageNumber: {pageNumber} | PageSize: {pageSize}", _collection, pageNumber,
                pageSize);
            try
            {
                var totalItems =
                    await Collection.CountDocumentsAsync(_clientSessionHandle, filter,
                        cancellationToken: cancellationToken);
                var items = await Collection.Find(_clientSessionHandle, filter)
                    .Skip((pageNumber - 1) * pageSize)
                    .Limit(pageSize)
                    .ToListAsync(cancellationToken: cancellationToken);
                Logger.Information("Paged entities retrieved by filter successfully from collection: {collection} | PageNumber: {pageNumber} | PageSize: {pageSize}", _collection,
                    pageNumber, pageSize);
                return new Pagination<T>(items, (int)totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error retrieving paged entities by filter from collection: {collection} | PageNumber: {pageNumber} | PageSize: {pageSize}", _collection,
                    pageNumber, pageSize);
                throw;
            }
        }

        public async System.Threading.Tasks.Task UpdateAsync(string id, T entity, CancellationToken cancellationToken)
        {
            Logger.Information("Updating entity with ID: {id} in collection: {collection}", id, _collection);
            try
            {
                entity.SetUpdateInfo();
                var objectId = new ObjectId(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);

                var updateDefinitionBuilder = Builders<T>.Update;
                var updateDefinition = new List<UpdateDefinition<T>>();
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    if (value != null)
                    {
                        updateDefinition.Add(updateDefinitionBuilder.Set(property.Name, value));
                    }
                }

                var update = updateDefinitionBuilder.Combine(updateDefinition);
                await Collection.UpdateOneAsync(_clientSessionHandle, filter, update, cancellationToken: cancellationToken);
                Logger.Information("Entity updated successfully with ID: {id} in collection: {collection}", id, _collection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error updating entity with ID: {id} in collection: {collection}", id, _collection);
                throw;
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            Logger.Information("Deleting entity with ID: {id} from collection: {collection}", id, _collection);
            try
            {
                var objectId = new ObjectId(id);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                await Collection.DeleteOneAsync(_clientSessionHandle, filter, cancellationToken: cancellationToken);
                Logger.Information("Entity deleted successfully with ID: {id} from collection: {collection}", id, _collection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error deleting entity with ID: {id} from collection: {collection}", id, _collection);
                throw;
            }
        }
    }
}