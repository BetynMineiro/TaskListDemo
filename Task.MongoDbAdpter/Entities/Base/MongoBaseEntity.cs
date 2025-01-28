using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Task.Domain.Entities.Base;

namespace Task.MongoDbAdpter.Entities.Base;

public class MongoBaseEntity : EntityBase
{
    [BsonId] public ObjectId Id { get; set; }

}