using Closest.Network.Location.API.Services.Helpers;
using Messages.Core;
using Messages.Core.Extensions;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class GetLocationDto
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public GetLocationDto(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static Response<GetLocationDto> Create(double latitude, double longitude)
        {
            var response = Response<GetLocationDto>.Create();

            var coordinatesAreValidResponse = CoordinateHelper.CoordinatesAreValidResponse(longitude, latitude);

            if (coordinatesAreValidResponse.HasError)
                return response.WithMessages(coordinatesAreValidResponse.Messages);

            return response.SetValue(new GetLocationDto(latitude, longitude));
        }
    }
}
