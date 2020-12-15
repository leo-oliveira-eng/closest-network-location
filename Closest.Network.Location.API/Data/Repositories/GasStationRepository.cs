using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Models;
using Closest.Network.Location.API.Settings.Contracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Data.Repositories
{
    public class GasStationRepository : IGasStationRepository
    {
        IMongoCollection<GasStation> Collection { get; }

        IClosestNetworkLocationSettings Settings { get; }

        public GasStationRepository(IClosestNetworkLocationSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Collection = database.GetCollection<GasStation>(settings.CollectionName);

            var locationIndexBuilder = Builders<GasStation>.IndexKeys;
            var indexModel = new CreateIndexModel<GasStation>(locationIndexBuilder.Geo2DSphere(_ => _.Address.Location));

            Collection.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false);
        }

        public async Task<List<GasStation>> GetAllAsync()
            => await Collection.FindAsync(_ => true)
                               .GetAwaiter()
                               .GetResult()
                               .ToListAsync();


    }
}
