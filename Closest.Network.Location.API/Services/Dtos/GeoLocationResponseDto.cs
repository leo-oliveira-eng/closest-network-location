using Newtonsoft.Json;
using System.Collections.Generic;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class GeoLocationResponseDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results")]
        public List<ResultDto> Results { get; set; }
    }
}
