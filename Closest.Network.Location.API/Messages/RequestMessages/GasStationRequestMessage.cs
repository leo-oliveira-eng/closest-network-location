using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.RequestMessages
{
    [DataContract, ]
    public class GasStationRequestMessage
    {
        [DataMember, JsonProperty("id")]
        public string ExternalId { get; set; }

        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        [DataMember, JsonProperty("telephone")]
        public string PhoneNumber { get; set; }

        [DataMember, JsonProperty("website_url")]
        public string SiteUrl { get; set; }

        [DataMember, JsonProperty("lat")]
        public double Latitude { get; set; }

        [DataMember, JsonProperty("lng")]
        public double Longitude { get; set; }

        [DataMember, JsonProperty("address")]
        public string StreetAddress { get; set; }

        [DataMember, JsonProperty("city")]
        public string City { get; set; }

        [DataMember, JsonProperty("state")]
        public string UF { get; set; }

        [DataMember, JsonProperty("postcode")]
        public string Cep { get; set; }
    }
}
