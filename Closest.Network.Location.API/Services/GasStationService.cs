using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Factories.Contracts;
using Closest.Network.Location.API.Messages.RequestMessages;
using Closest.Network.Location.API.Models;
using Closest.Network.Location.API.Services.Dtos;
using Closest.Network.Location.API.Services.ExternalServices.Contracts;
using Closest.Network.Location.API.Services.Mappers;
using Closest.Network.Location.API.Settings.Contracts;
using Messages.Core;
using Messages.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Services
{
    public class GasStationService
    {
        IClosestNetworkLocationSettings Settings { get; }
        
        IGasStationRepository GasStationRepository { get; }

        IGeolocationExternalService GeolocationExternalService { get; }

        IGasStationFactory GasStationFactory { get; }

        public GasStationService(IClosestNetworkLocationSettings settings
            , IGasStationRepository gasStationRepository
            , IGeolocationExternalService geolocationExternalService
            , IGasStationFactory gasStationFactory)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            GasStationRepository = gasStationRepository ?? throw new ArgumentNullException(nameof(gasStationRepository));
            GeolocationExternalService = geolocationExternalService ?? throw new ArgumentNullException(nameof(geolocationExternalService));
            GasStationFactory = gasStationFactory ?? throw new ArgumentNullException(nameof(gasStationFactory));
        }

        public async Task<Response> ImportAsync(ImportGasStationRequestMessage requestMessage)
        {
            var response = Response.Create();

            if (requestMessage == null)
                return response.WithBusinessError("Registros inválidos");

            var createGasStationsResponse = await CreateGasStationsAsync(requestMessage.GasStations.ToGasStationsDto());

            if (createGasStationsResponse.HasError)
                return response.WithMessages(createGasStationsResponse.Messages);

            return response;
        }

        private async Task<Response> CreateGasStationsAsync(List<GasStationDto> gasStationDtos)
        {
            var response = Response.Create();

            foreach(var dto in gasStationDtos)
            {
                var gasStation = await GasStationRepository.FindByExternalIdAsync(dto.ExternalId);

                var addOrUpdateResponse = gasStation.HasValue
                    ? await UpdateGasStationAsync(gasStation, dto)
                    : await AddGasStationAsync(dto);

                if (addOrUpdateResponse.HasError)
                    response.WithMessages(addOrUpdateResponse.Messages);
            }

            return response;
        }

        private async Task<Response> UpdateGasStationAsync(GasStation gasStation, GasStationDto dto)
        {
            var response = Response.Create();

            var newAddressResponse = Address.Create(dto.Address.Cep, dto.Address.StreetAddress, dto.Address.City, dto.Address.UF, dto.Address.Complement);

            if (newAddressResponse.HasError)
                return response.WithMessages(newAddressResponse.Messages);

            if (!newAddressResponse.Equals(gasStation.Address))
            {
                var address = $"{newAddressResponse.Data.Value.StreetAddress}"
                    + $"+{(!string.IsNullOrEmpty(newAddressResponse.Data.Value.Complement) ? $"{newAddressResponse.Data.Value.Complement}," : string.Empty)}"
                    + $"+{newAddressResponse.Data.Value.City}"
                    + $"+{newAddressResponse.Data.Value.UF}"
                    + $"CEP: {newAddressResponse.Data.Value.Cep}";

                var geocodingResponse = await GeolocationExternalService.GetGeolocationAsync(address);

                if (geocodingResponse.HasError)
                    return response.WithMessages(geocodingResponse.Messages);

                newAddressResponse.Data.Value.SetLocation(geocodingResponse.Data.Value.Longitude, geocodingResponse.Data.Value.Latitude);
            }

            return gasStation.Update(dto, newAddressResponse);
        }

        private async Task<Response> AddGasStationAsync(GasStationDto dto)
            => await GasStationFactory.CreateAsync(dto);
    }
}
