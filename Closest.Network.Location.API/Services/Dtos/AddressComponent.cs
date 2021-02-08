using Newtonsoft.Json;
using System.Collections.Generic;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class AddressComponent
    {
        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }
}
