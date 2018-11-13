using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Infrastructure.Repositories
{
    public class MongoLogRepository : MongoRepository<Log>, ILogRepository
    {
        public MongoLogRepository(IMongoDatabase Db) : base(Db) { }

        public async Task<Log> DeleteAsync(string logID)
            => await Collection.FindOneAndDeleteAsync(en => en.ID == logID);

        public async Task<Log> GetByIDAsync(string logID) => await (await Collection.FindAsync(u => u.ID == logID)).FirstOrDefaultAsync();

        public async Task<bool> IsExistsAsync(string logID) => (await GetByIDAsync(logID)) != null;

        public async Task<Log> UpdateAsync(Log log)
        {
            log.DateModified = DateTime.UtcNow;

            var fields_dictionary = log.ToBsonDocument().ToDictionary();

            UpdateDefinition<Log> update_query = null;

            foreach (var entry in fields_dictionary)
            {
                if (update_query == null)
                    update_query = UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]);
                else
                    update_query = UpdateBuilder.Combine(update_query, UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]));
            }

            try
            {

                var result = await Collection.UpdateOneAsync(e => e.ID == log.ID, update_query);

                if (result.ModifiedCount == 0)
                    throw new ApplicationException("Can't Update Log ! Please Try Again Later .");

                else return log;
            }
            catch
            {
                throw new ApplicationException("Update Operation Failed");
            }
        }
    }
}
