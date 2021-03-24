using Messages.Core;
using Messages.Core.Extensions;

namespace Closest.Network.Location.API.Services.Helpers
{
    public static class CoordinateHelper
    {
        public static Response CoordinatesAreValidResponse(double longitude, double latitude)
        {
            var response = Response.Create();

            if (!LatitudeIsValid(latitude))
                response.WithBusinessError(nameof(latitude), "Coordenadas de latitude devem estar entre -90 e 90");

            if (!LongitudeIsValid(longitude))
                response.WithBusinessError(nameof(longitude), "Coordenadas de longitude devem estar entre -180 e 180");

            return response;
        }

        public static bool LatitudeIsValid(double latitude) => latitude <= 90 && latitude >= -90;

        public static bool LongitudeIsValid(double longitude) => longitude <= 180 && longitude >= -180;
    }
}
