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
    public class MongoUserRepository : MongoRepository<User>, IUserRepository
    {

        public MongoUserRepository(IMongoDatabase Db) : base(Db) { }

        public async Task<User> DeleteAsync(string userID)
            => await Collection.FindOneAndDeleteAsync(en => en.ID == userID);

        public async Task<User> UpdateAsync(User user)
        {

            user.DateModified = DateTime.UtcNow;

            var fields_dictionary = user.ToBsonDocument().ToDictionary();

            UpdateDefinition<User> update_query = null;

            foreach (var entry in fields_dictionary)
            {
                if (update_query == null)
                    update_query = UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]);
                else
                    update_query = UpdateBuilder.Combine(update_query, UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]));
            }

            try
            {
                var result = await Collection.UpdateOneAsync(e => e.ID == user.ID, update_query);

                if (result.ModifiedCount == 0)
                    throw new ApplicationException("Can't Update Log ! Please Try Again Later .");

                else return user;

            }
            catch
            {
                throw new ApplicationException("Update Operation Failed");
            }
        }

        public async Task<User> GetByIDAsync(string userID) => await (await Collection.FindAsync(u => u.ID == userID)).FirstOrDefaultAsync();

        public async Task<bool> IsExistsAsync(string userID) => (await GetByIDAsync(userID)) != null;
    }

}
