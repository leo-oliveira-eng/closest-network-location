using Closest.Network.Location.API.Services.Dtos;
using Messages.Core;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Services.ExternalServices.Contracts
{
    public interface IGeolocationExternalService
    {
        Task<Response<LocationDto>> GetGeolocationAsync(string address);
    }
}
