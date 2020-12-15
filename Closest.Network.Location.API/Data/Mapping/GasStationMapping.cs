using Closest.Network.Location.API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace Closest.Network.Location.API.Data.Mapping
{
    public static class GasStationMapping
    {
        public static void MapGasStation()
        {
            BsonClassMap.RegisterClassMap<GasStation>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.MapMember(x => x.CreatedAt).SetDefaultValue(DateTime.Now);
                cm.MapMember(x => x.LastUpdate).SetDefaultValue(DateTime.Now);
            });
        }
    }
}
