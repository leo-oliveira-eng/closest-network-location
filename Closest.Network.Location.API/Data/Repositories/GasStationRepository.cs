using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Models;
using Closest.Network.Location.API.Settings.Contracts;
using Messages.Core;
using Messages.Core.Extensions;
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

        public async Task<Maybe<GasStation>> FindByExternalIdAsync(string externalId)
            => await Collection.FindAsync(gasStation => gasStation.ExternalId.Equals(externalId))
                               .GetAwaiter()
                               .GetResult()
                               .FirstOrDefaultAsync();

        public async Task<Response<GasStation>> AddAsync(GasStation gasStation)
        {
            var response = Response<GasStation>.Create();

            try
            {
                await Collection.InsertOneAsync(gasStation);

                return response.SetValue(gasStation);
            }
            catch (Exception ex)
            {
                return response.WithCriticalError(ex.ToString());
            }
        }

        public async Task<UpdateResult> UpdadeAsync(GasStation gasStation)
        {
            var filter = Builders<GasStation>.Filter.Eq(x => x.Id, gasStation.Id);
            var update = Builders<GasStation>.Update
                .Set(nameof(gasStation.ExternalId), gasStation.ExternalId)
                .Set(nameof(gasStation.Name), gasStation.Name)
                .Set(nameof(gasStation.PhoneNumber), gasStation.PhoneNumber)
                .Set(nameof(gasStation.SiteUrl), gasStation.SiteUrl)
                .Set(nameof(gasStation.Address), gasStation.Address)
                .CurrentDate(nameof(gasStation.LastUpdate));

            return await Collection.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> DeleteAsync(GasStation gasStation)
        {
            var filter = Builders<GasStation>.Filter.Eq(_ => _.ExternalId, gasStation.ExternalId);
            var update = Builders<GasStation>.Update
                .CurrentDate(nameof(gasStation.DeletedAt))
                .CurrentDate(nameof(gasStation.LastUpdate));

            return await Collection.UpdateOneAsync(filter, update);
        }
    }
}