using LogsDemo.Domain.Interfaces;
using LogsDemo.Infrastructure.Mongo.Utilities;
using LogsDemo.SharedKernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Infrastructure.Mongo.Repositories
{
    public abstract class MongoRepository<T> : IRepository<T> where T : class
    {


        private IMongoDatabase Db { get; set; }
        private IMongoClient Client { get; set; }

        /// <summary>
        /// Connect to default server 
        /// </summary>
        /// <param name="DbName">Database Name</param>
        protected MongoRepository(IMongoClient Client, string DbName)
        {
            try
            {
                this.Client = Client ?? throw new ArgumentNullException(nameof(IMongoClient));

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

        protected IMongoCollection<T> Collection { get; private set; }

        protected FilterDefinitionBuilder<T> FilterBuilder { get; set; }

        protected UpdateDefinitionBuilder<T> UpdateBuilder { get; set; }



        private void Initialize()
        {

            this.Db = Db;

            Collection = Db.GetCollection<T>(typeof(T).Name);

            FilterBuilder = Builders<T>.Filter;

            UpdateBuilder = Builders<T>.Update;

        }

        public async Task<T> CreateAsync(T Entity)
        {
            await Collection.InsertOneAsync(Entity);

            return Entity;
        }

        public async Task<IList<T>> ListAsync()
            => await (await Collection.FindAsync<T>(new BsonDocument())).ToListAsync();

        public async Task<IList<T>> GetOnAsync(Expression<Func<T, bool>> expression)
            => await (await Collection.FindAsync<T>(expression)).ToListAsync();
    }
}
