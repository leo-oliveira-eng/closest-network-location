﻿using Closest.Network.Location.API.Models;
using Messages.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Data.Contracts
{
    public interface IGasStationRepository
    {
        Task<List<GasStation>> GetAllAsync();

        Task<Maybe<GasStation>> FindByExternalIdAsync(string externalId);

        Task<Response<GasStation>> AddAsync(GasStation gasStation);
    }
}
