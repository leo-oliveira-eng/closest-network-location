using Closest.Network.Location.API.Models;
using Closest.Network.Location.API.Services.Dtos;
using Messages.Core;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Data.Contracts
{
    public interface IGasStationRepository
    {
        Task<List<GasStation>> GetAllAsync();

        Task<Maybe<GasStation>> FindByExternalIdAsync(string externalId);

        Task<Response<GasStation>> AddAsync(GasStation gasStation);

        Task<UpdateResult> UpdadeAsync(GasStation gasStation);

        Task<UpdateResult> DeleteAsync(GasStation gasStation);

        Task<Response<List<GasStation>>> GetGasStationsByLocationAsync(GetLocationDto location);
    }
}
