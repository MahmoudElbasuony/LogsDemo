using LogsDemo.Models;
using LogsDemo.Domain.Interfaces;
using LogsDemo.SharedKernel.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace LogsDemo.Infrastructure.Mongo.Repositories
{
    public class MongoUserRepository : MongoRepository<User<string>>, IUserRepository<string>
    {

        private MongoUserRepository(IMongoClient client, string DbName) : base(client, DbName)
        {
            if (string.IsNullOrWhiteSpace(DbName))
                throw new ArgumentNullException(nameof(DbName));
        }



        public async Task<User<string>> DeleteAsync(string userID)
        {
            return await Collection.FindOneAndDeleteAsync(en => en.ID  == userID );
        }

        public async Task<User<string>> UpdateAsync(User<string> user)
        {

            user.DateModified = DateTime.UtcNow;

            var fields_dictionary = user.ToBsonDocument().ToDictionary();

            UpdateDefinition<User<string>> update_query = null;

            foreach (var entry in fields_dictionary)
            {
                if (update_query == null)
                    update_query = UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]);
                else
                    update_query = UpdateBuilder.Combine(update_query, UpdateBuilder.Set(entry.Key, fields_dictionary[entry.Key]));
            }

            try
            {
                var result = await Collection.UpdateOneAsync(e => e.ID  == user.ID , update_query);

                if (result.ModifiedCount == 0)
                    throw new CustomException("Can't Update Log ! Please Try Again Later .");

                else return user;

            }
            catch
            {
                throw new CustomException("Update Operation Failed");
            }
        }

        public async Task<User<string>> GetByIDAsync(string userID)
        {

            return await (await Collection.FindAsync(u => u.ID  == userID )).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(string userID)
        {
            return (await GetByIDAsync(userID)) != null;
        }



        public static IUserRepository<string> Create(IMongoClient client, string DbName)
        {
            return new MongoUserRepository(client, DbName);
        }



    }

}
