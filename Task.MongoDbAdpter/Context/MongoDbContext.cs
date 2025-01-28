using MongoDB.Driver;

namespace Task.MongoDbAdpter.Context;

public class MongoDbContext
{
    protected static string Database;

    public static MongoClient BuildMongoConnection(string connection, string databaseName)
    {
        Database = databaseName;
        return new MongoClient(connection);
    }
}