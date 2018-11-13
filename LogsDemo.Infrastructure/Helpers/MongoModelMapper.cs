using LogsDemo.Domain.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Infrastructure.Helpers
{
    internal static class MongoModelMapper
    {
        internal static void Map()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity<string>)))
            {
                BsonClassMap.RegisterClassMap<BaseEntity<string>>((map) =>
                {
                    map.AddKnownType(typeof(User));
                    map.AddKnownType(typeof(Log));
                    map.AutoMap();
                    map.MapIdMember(en => en.ID).SetIdGenerator(new StringObjectIdGenerator());
                });
            }

          
        }
    }
}
