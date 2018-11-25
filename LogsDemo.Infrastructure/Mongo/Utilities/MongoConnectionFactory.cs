using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Infrastructure.Mongo.Utilities
{
    public static class MongoConnectionFactory
    {
        private static IMongoClient mongoClient;

        private static readonly object padlock = new object();

        public static IMongoClient GetConnection(string ConnectionString = null)
        {
            if (mongoClient == null)
                lock (padlock)
                {
                    if (ConnectionString != null)
                    {
                        mongoClient = mongoClient ?? new MongoClient(ConnectionString);
                    }
                    else
                    {
                        mongoClient = mongoClient ?? new MongoClient();
                    }
                }

            return mongoClient;

        }
    }
}
