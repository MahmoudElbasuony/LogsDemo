using LogsDemo.Domain.Interfaces;
using LogsDemo.Models;
using LogsDemo.SharedKernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace LogsDemo.Infrastructure.Mongo.Repositories
{
    public class MongoLogRepository : MongoRepository<Log<string>>, ILogRepository<string>
    {
        public MongoLogRepository(IMongoClient client, string DbName) : base(client, DbName)
        {
            if (string.IsNullOrWhiteSpace(DbName))
                throw new ArgumentNullException(nameof(DbName));
        }



        public async Task<Log<string>> DeleteAsync(string logID)
        {
            return await Collection.FindOneAndDeleteAsync(en => en.ID  == logID );
        }

        public async Task<Log<string>> GetByIDAsync(string logID)
        {
            return await (await Collection.FindAsync(u => u.ID  == logID )).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(string logID)
        {
            return (await GetByIDAsync(logID)) != null;
        }

        public async Task<Log<string>> UpdateAsync(Log<string> log)
        {
            log.DateModified = DateTime.UtcNow;

            var fields_dictionary = log.ToBsonDocument().ToDictionary();

            UpdateDefinition<Log<string>> update_query = null;

            foreach (var entry in fields_dictionary)
            {
                if (update_query == null)
                    update_query = UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]);
                else
                    update_query = UpdateBuilder.Combine(update_query, UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]));
            }

            try
            {

                var result = await Collection.UpdateOneAsync(e => e.ID  == log.ID , update_query);

                if (result.ModifiedCount == 0)
                    throw new CustomException("Can't Update Log ! Please Try Again Later .");

                else return log;
            }
            catch
            {
                throw new CustomException("Update Operation Failed");
            }
        }

        public static ILogRepository<string> Create(IMongoClient client, string DbName, string ConnectionString = null)
        {
            return new MongoLogRepository(client, DbName);
        }

     


    }
}
