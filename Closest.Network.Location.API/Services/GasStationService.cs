using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Factories.Contracts;
using Closest.Network.Location.API.Messages.RequestMessages;
using Closest.Network.Location.API.Models;
using Closest.Network.Location.API.Services.Dtos;
using Closest.Network.Location.API.Services.ExternalServices.Contracts;
using Closest.Network.Location.API.Services.Mappers;
using Messages.Core;
using Messages.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Services
{
    public class GasStationService
    {
        IGasStationRepository GasStationRepository { get; }

        IGeolocationExternalService GeolocationExternalService { get; }

        IGasStationFactory GasStationFactory { get; }

        public GasStationService(IGasStationRepository gasStationRepository
            , IGeolocationExternalService geolocationExternalService
            , IGasStationFactory gasStationFactory)
        {
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

                var updatedAddressResponse = newAddressResponse.Data.Value.SetLocation(geocodingResponse.Data.Value.Longitude, geocodingResponse.Data.Value.Latitude);

                if (updatedAddressResponse.HasError)
                    return response.WithMessages(updatedAddressResponse.Messages);
            }

            var updateGasStationResponse =  gasStation.Update(dto, newAddressResponse);

            if (updateGasStationResponse.HasError)
                return updateGasStationResponse;

            if (!(await GasStationRepository.UpdadeAsync(gasStation)).IsAcknowledged)
                return response.WithCriticalError($"Failed to save gas station {gasStation.ExternalId}");

            return response;
        }

        private async Task<Response> AddGasStationAsync(GasStationDto dto)
            => await GasStationFactory.CreateAsync(dto);

        public async Task<Response> RemoveAsync(string externalId)
        {
            var response = Response.Create();

            if (string.IsNullOrEmpty(externalId))
                return response.WithBusinessError($"{nameof(externalId)} is invalid");

            var gasStation = await GasStationRepository.FindByExternalIdAsync(externalId);

            if (!gasStation.HasValue)
                return response.WithBusinessError("Not found");

            if (!(await GasStationRepository.DeleteAsync(gasStation.Value)).IsAcknowledged)
                return response.WithCriticalError($"Failed to delete gas station {externalId}");

            return response;
        }
    }
}
