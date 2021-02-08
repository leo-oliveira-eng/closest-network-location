using Closest.Network.Location.API.Messages.RequestMessages;
using Closest.Network.Location.API.Services.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Closest.Network.Location.API.Services.Mappers
{
    public static class GasStationMapper
    {
        public static List<GasStationDto> ToGasStationsDto(this List<GasStationRequestMessage> gasStations)
        {
            if (gasStations == null)
                return new List<GasStationDto>();

            return gasStations.Select(gasStation => new GasStationDto
            {
                ExternalId = gasStation.ExternalId,
                Name = gasStation.Name,
                PhoneNumber = gasStation.PhoneNumber,
                SiteUrl = gasStation.SiteUrl,
                Address = new AddressDto
                {
                    Cep = gasStation.Cep,

                }
            }).ToList();
        }
    }
}
