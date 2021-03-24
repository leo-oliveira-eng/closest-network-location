using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.ResponseMessages
{
    [DataContract]
    public class LocationResponseMessage
    {
        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }
    }
}
