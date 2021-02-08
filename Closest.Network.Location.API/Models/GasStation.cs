using Closest.Network.Location.API.Services.Dtos;
using Messages.Core;
using Messages.Core.Extensions;
using System;

namespace Closest.Network.Location.API.Models
{
    public class GasStation
    {
        #region Properties

        public string Id { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public DateTime LastUpdate { get; private set; } = DateTime.Now;

        public DateTime? DeletedAt { get; private set; }

        public bool Deleted => DeletedAt.HasValue;

        public string ExternalId { get; private set; }

        public string Name { get; private set; }

        public string PhoneNumber { get; private set; }

        public string SiteUrl { get; private set; }

        public Address Address { get; private set; }

        #endregion

        #region Constructors

        [Obsolete("Created only for EF", true)]
        public GasStation() { }

        public GasStation(string externalId, string name, string phoneNumber, string siteUrl, Address address)
        {
            ExternalId = externalId;
            Name = name;
            PhoneNumber = phoneNumber;
            SiteUrl = siteUrl;
            Address = address;
        }

        #endregion

        #region Methods

        public static Response<GasStation> Create(GasStationDto dto, Address address)
        {
            var response = Response<GasStation>.Create();

            var dataIsValidResponse = IsValidForGasStation(dto, address);

            if (dataIsValidResponse.HasError)
                return response.WithMessages(dataIsValidResponse.Messages);

            return response.SetValue(new GasStation(dto.ExternalId, dto.Name, dto.PhoneNumber, dto.SiteUrl, address));
        }

        static Response IsValidForGasStation(GasStationDto dto, Address address)
        {
            var response = Response.Create();

            if (string.IsNullOrEmpty(dto.ExternalId))
                response.WithBusinessError(nameof(dto.ExternalId), $"O campo {nameof(dto.ExternalId)} é obrigatório. Rever o credenciado {dto.ExternalId}");

            if (string.IsNullOrEmpty(dto.Name))
                response.WithBusinessError(nameof(dto.Name), $"O campo {nameof(dto.Name)} é obrigatório. Rever o credenciado {dto.ExternalId}");

            if (string.IsNullOrEmpty(dto.PhoneNumber))
                response.WithBusinessError(nameof(dto.PhoneNumber), $"O campo {nameof(dto.PhoneNumber)} é obrigatório. Rever o credenciado {dto.ExternalId}");

            if (address is null)
                response.WithBusinessError(nameof(address), $"Endereço inválido.. Rever o credenciado {dto.ExternalId}");

            return response;
        }

        public Response Update(GasStationDto dto, Address address)
        {
            var response = Response.Create();

            var dataIsValidResponse = IsValidForGasStation(dto, address);

            if (dataIsValidResponse.HasError)
                return response.WithMessages(dataIsValidResponse.Messages);

            Name = dto.Name;
            Address = address;
            PhoneNumber = dto.PhoneNumber;
            SiteUrl = dto.SiteUrl;

            return response;
        }

        #endregion

        #region Conversion Operators

        public static implicit operator GasStation(Maybe<GasStation> entity) => entity.Value;

        public static implicit operator GasStation(Response<GasStation> entity) => entity.Data;

        #endregion
    }
}
