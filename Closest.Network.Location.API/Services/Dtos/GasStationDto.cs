using Closest.Network.Location.API.Models;

namespace Closest.Network.Location.API.Services.Dtos
{
    public class GasStationDto
    {
        public string ExternalId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string SiteUrl { get; set; }

        public Address Address { get; set; }
    }
}
