using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.RequestMessages
{
    [DataContract]
    public class GetGasStationRequestMessage
    {
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }
    }
}
