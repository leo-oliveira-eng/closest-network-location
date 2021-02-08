using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.RequestMessages
{
    [DataContract]
    public class ImportGasStationRequestMessage
    {
        [DataMember]
        public List<GasStationRequestMessage> GasStations { get; set; }
    }
}
