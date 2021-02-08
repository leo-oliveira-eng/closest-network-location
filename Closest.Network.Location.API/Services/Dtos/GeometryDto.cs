using Newtonsoft.Json;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class GeometryDto
    {
        [JsonProperty("location")]
        public LocationDto Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("viewport")]
        public FrameDto Viewport { get; set; }

        [JsonProperty("bounds")]
        public FrameDto Bounds { get; set; }
    }
}
