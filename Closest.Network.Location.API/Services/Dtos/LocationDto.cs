using Newtonsoft.Json;
using System;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class LocationDto
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}
