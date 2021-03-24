using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.ResponseMessages
{
    [DataContract]
    public class GasStationResponseMessage
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string ExternalId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string SiteUrl { get; set; }

        [DataMember]
        public AddressResponseMessage Address { get; set; }
    }
}
