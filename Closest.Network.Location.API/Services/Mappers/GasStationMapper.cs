using Closest.Network.Location.API.Messages.RequestMessages;
using Closest.Network.Location.API.Messages.ResponseMessages;
using Closest.Network.Location.API.Models;
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
                    StreetAddress = gasStation.StreetAddress,
                    City = gasStation.City,
                    UF = gasStation.UF,
                    Latitude = gasStation.Latitude,
                    Longitude = gasStation.Longitude
                }
            }).ToList();
        }

        public static GetGasStationResponseMessage ToGetGasStationResponseMessage(this List<GasStation> gasStations, GetLocationDto location)
            => new GetGasStationResponseMessage
            {
                GasStations = !gasStations.Any() ? new List<GasStationResponseMessage>() : gasStations.Select(gasStation => new GasStationResponseMessage
                {
                    Id = gasStation.Id,
                    ExternalId = gasStation.ExternalId,
                    Name = gasStation.Name,
                    PhoneNumber = gasStation.PhoneNumber,
                    SiteUrl = gasStation.SiteUrl,
                    Address = new AddressResponseMessage
                    {
                        StreetAddress = gasStation.Address.StreetAddress,
                        Cep = gasStation.Address.Cep,
                        Complement = gasStation.Address.Complement,
                        City = gasStation.Address.City,
                        UF = gasStation.Address.UF,
                        Latitude = gasStation.Address.Location[1],
                        Longitude = gasStation.Address.Location[0]
                    }
                }).ToList(),
                ReferenceLocation = new LocationResponseMessage
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                }
            };
    }
}
