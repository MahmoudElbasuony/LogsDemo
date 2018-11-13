using LogsDemo.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Infrastructure.Repositories
{
    public abstract class MongoRepository<T> : IRepository<T> where T : class
    {
        protected IMongoDatabase Db { get; }

        protected IMongoCollection<T> Collection { get; }

        protected FilterDefinitionBuilder<T> FilterBuilder { get; set; }

        protected UpdateDefinitionBuilder<T> UpdateBuilder { get; set; }


        public MongoRepository(IMongoDatabase Db)
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
            => await(await Collection.FindAsync<T>(expression)).ToListAsync();
    }
}
