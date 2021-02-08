using Closest.Network.Location.API.Services.Dtos;
using Messages.Core;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Factories.Contracts
{
    public interface IGasStationFactory
    {
        Task<Response> CreateAsync(GasStationDto dto);
    }
}
