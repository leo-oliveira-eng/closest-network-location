using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Factories.Contracts;
using Closest.Network.Location.API.Models;
using Closest.Network.Location.API.Services.Dtos;
using Closest.Network.Location.API.Services.ExternalServices.Contracts;
using Messages.Core;
using Messages.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace Closest.Network.Location.API.Factories
{
    public class GasStationFactory : IGasStationFactory
    {
        IGeolocationExternalService GeolocationExternalService { get; }

        IGasStationRepository GasStationRepository { get; }

        public GasStationFactory(IGeolocationExternalService geolocationExternalService, IGasStationRepository gasStationRepository)
        {
            GeolocationExternalService = geolocationExternalService ?? throw new ArgumentNullException(nameof(geolocationExternalService));
            GasStationRepository = gasStationRepository ?? throw new ArgumentNullException(nameof(gasStationRepository));
        }

        public async Task<Response> CreateAsync(GasStationDto dto)
        {
            var response = Response.Create();

            var addressResponse = await CreateAddressAsync(dto);

            if (addressResponse.HasError)
                return response.WithMessages(addressResponse.Messages);

            var gasStationResponse =  GasStation.Create(dto, addressResponse);

            if (gasStationResponse.HasError)
                return response.WithMessages(gasStationResponse.Messages);

            var addGasStationResponse = await GasStationRepository.AddAsync(gasStationResponse);

            if (addGasStationResponse.HasError)
                return response.WithMessages(addGasStationResponse.Messages);

            return response;
        }

        private async Task<Response<Address>> CreateAddressAsync(GasStationDto dto)
        {
            var response = Response<Address>.Create();

            var addressResponse = Address.Create(dto.Address.Cep, dto.Address.StreetAddress, dto.Address.City, dto.Address.UF, dto.Address.Complement);

            if (addressResponse.HasError)
                return addressResponse;

            var address = $"{addressResponse.Data.Value.StreetAddress}"
                    + $"+{(!string.IsNullOrEmpty(addressResponse.Data.Value.Complement) ? $"{addressResponse.Data.Value.Complement}," : string.Empty)}"
                    + $"+{addressResponse.Data.Value.City}"
                    + $"+{addressResponse.Data.Value.UF}"
                    + $"CEP: {addressResponse.Data.Value.Cep}";

            var geocodingResponse = await GeolocationExternalService.GetGeolocationAsync(address);

            if (geocodingResponse.HasError)
                return response.WithMessages(geocodingResponse.Messages);

            var setCordinatesResponse = addressResponse.Data.Value.SetLocation(geocodingResponse.Data.Value.Longitude, geocodingResponse.Data.Value.Latitude);

            if (setCordinatesResponse.HasError)
                return response.WithMessages(setCordinatesResponse.Messages);

            return response.SetValue(addressResponse);
        }
    }
}
