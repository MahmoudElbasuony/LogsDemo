using LogsDemo.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Infrastructure.Mongo.Utilities
{
    internal static class MongoModelMapper
    {
        internal static void Map()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity<string>)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity<string>>((map) =>
                {
                    map.AddKnownType(typeof(User<string>));
                    map.AddKnownType(typeof(Log<string>));
                    map.AutoMap();
                    map.MapIdMember(en => en.ID).SetIdGenerator(new StringObjectIdGenerator());
                });
            }

          
        }
    }
}
