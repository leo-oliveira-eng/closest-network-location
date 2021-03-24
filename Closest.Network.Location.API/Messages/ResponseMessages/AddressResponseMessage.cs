using System.Runtime.Serialization;

namespace Closest.Network.Location.API.Messages.ResponseMessages
{
    [DataContract]
    public class AddressResponseMessage
    {
        [DataMember]
        public string Cep { get; set; }

        [DataMember]
        public string StreetAddress { get; set; }

        [DataMember]
        public string Complement { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string UF { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }
    }
}
