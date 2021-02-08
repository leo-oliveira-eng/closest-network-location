using Newtonsoft.Json;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class FrameDto
    {
        [JsonProperty("northeast")]
        public LocationDto Northeast { get; set; }

        [JsonProperty("southwest")]
        public LocationDto Southwest { get; set; }
    }
}
