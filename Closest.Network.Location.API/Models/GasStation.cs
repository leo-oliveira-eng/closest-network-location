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

        public static Response<GasStation> Create(GasStationDto dto)
        {
            var response = Response<GasStation>.Create();

            var dtoIsValidResponse = IsValidForGasStation(dto);

            if (dtoIsValidResponse.HasError)
                return response.WithMessages(dtoIsValidResponse.Messages);

            return response.SetValue(new GasStation(dto.ExternalId, dto.Name, dto.PhoneNumber, dto.SiteUrl, dto.Address));
        }

        static Response IsValidForGasStation(GasStationDto dto)
        {
            var response = Response.Create();

            if (string.IsNullOrEmpty(dto.ExternalId))
                response.WithBusinessError(nameof(dto.ExternalId), $"O campo {nameof(dto.ExternalId)} é obrigatório. Rever o credenciado {dto.ExternalId}");

            if (string.IsNullOrEmpty(dto.Name))
                response.WithBusinessError(nameof(dto.Name), $"O campo {nameof(dto.Name)} é obrigatório. Rever o credenciado {dto.ExternalId}");

            if (string.IsNullOrEmpty(dto.PhoneNumber))
                response.WithBusinessError(nameof(dto.PhoneNumber), $"O campo {nameof(dto.PhoneNumber)} é obrigatório. Rever o credenciado {dto.ExternalId}");

            if (dto.Address is null)
                response.WithBusinessError(nameof(dto.Address), $"Endereço inválido.. Rever o credenciado {dto.ExternalId}");

            return response;
        }

        public Response Update(GasStationDto dto)
        {
            var response = Response.Create();

            var dtoIsValidResponse = IsValidForGasStation(dto);

            if (dtoIsValidResponse.HasError)
                return response.WithMessages(dtoIsValidResponse.Messages);

            Name = dto.Name;
            Address = dto.Address;
            PhoneNumber = dto.PhoneNumber;
            SiteUrl = dto.SiteUrl;

            return response;
        }

        #endregion
    }
}
