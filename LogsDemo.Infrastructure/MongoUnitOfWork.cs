using LogsDemo.Domain.Interfaces;
using LogsDemo.Infrastructure.Helpers;
using LogsDemo.Infrastructure.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Infrastructure.Contexts
{
    public class MongoUnitOfWork : ILogSystemUnitOfWork
    {
        public ILogRepository LogRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        private static MongoClient Client { get; set; }

        private IMongoDatabase Db { get; set; }


        /// <summary>
        /// Connect to default server 
        /// </summary>
        /// <param name="DbName">Database Name</param>
        private MongoUnitOfWork(string DbName)
        {
            try
            {
                Client = Client ?? new MongoClient();

                Db = GetMongoDatabase(DbName);

                Initialize();
            }
            catch
            {
                throw new Exception("Error To Connect to Database Server");
            }

        }

        /// <summary>
        /// Connect to server using provided connection string
        /// </summary>
        /// <param name="ConnectionString">Connection string for the server</param>
        /// <param name="DbName">Database Name</param>
        private MongoUnitOfWork(string ConnectionString, string DbName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ConnectionString))
                    throw new ArgumentNullException();

                Client = Client ?? new MongoClient(ConnectionString);

                Db = GetMongoDatabase(DbName);

                Initialize();
            }
            catch
            {
                throw new Exception("Error To Connect to Database Server");
            }

        }

        /// <summary>
        /// Get database reference by providing database name 
        /// </summary>
        /// <param name="DbName">Database Name</param>
        /// <returns></returns>
        private IMongoDatabase GetMongoDatabase(string DbName)
        {
            // config models mapping with mongo serializer
            MongoModelMapper.Map();

            if (Client != null)
                return Client.GetDatabase(DbName, new MongoDatabaseSettings { ReadEncoding = new UTF8Encoding(), WriteEncoding = new UTF8Encoding() });
            return null;
        }

        /// <summary>
        /// Initializes Repositories
        /// </summary>
        private void Initialize()
        {
            UserRepository = new MongoUserRepository(Db);

            LogRepository = new MongoLogRepository(Db);

        }

        public void Dispose()
        {
            // -------- 
        }


        public static MongoUnitOfWork Create(string DbName)
        {
            if (string.IsNullOrWhiteSpace(DbName))
                throw new ApplicationException("Database Name required");

            return new MongoUnitOfWork(DbName);
        }

        


    }
}
