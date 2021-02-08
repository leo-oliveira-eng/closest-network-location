using Newtonsoft.Json;
using System.Collections.Generic;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class ResultDto
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("geometry")]
        public GeometryDto Geometry { get; set; }
    }
}
