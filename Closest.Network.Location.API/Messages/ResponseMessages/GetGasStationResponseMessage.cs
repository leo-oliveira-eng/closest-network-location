using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.ResponseMessages
{
    [DataContract]
    public class GetGasStationResponseMessage
    {
        [DataMember]
        public LocationResponseMessage ReferenceLocation { get; set; }

        [DataMember]
        public List<GasStationResponseMessage> GasStations { get; set; }
    }
}
