using Closest.Network.Location.API.Services.Dtos;

namespace Closest.Network.Location.API.Services.Mappers
{
    public static class LocationMapper
    {
        public static GetLocationDto ToGetLocationDto(this LocationDto location)
            => new GetLocationDto(location.Latitude, location.Longitude);
    }
}
