using Messages.Core;
using Messages.Core.Extensions;
using System;

namespace Closest.Network.Location.API.Models
{
    public class Address
    {
        #region Properties

        public string Cep { get; private set; }

        public string StreetAddress { get; private set; }

        public string Complement { get; private set; }

        public string City { get; private set; }

        public string UF { get; private set; }

        public double[] Location { get; set; }

        #endregion

        #region Constructors

        [Obsolete("Created only for EF", true)]
        public Address() { }

        public Address(string cep, string street, string complement, string city, string uF)
        {
            Cep = cep;
            StreetAddress = street;
            Complement = complement;
            City = city;
            UF = uF;
        }

        #endregion

        #region Methods

        public static Response<Address> Create(string cep, string streetAddress, string city, string uF, string complement = "")
        {
            var response = Response<Address>.Create();

            var createIsValidResponse = AddressIsValidResponse(cep, streetAddress, city, uF);

            if (createIsValidResponse.HasError)
                return response.WithMessages(createIsValidResponse.Messages);

            return response.SetValue(new Address(cep, streetAddress, complement, city, uF));
        }

        static Response AddressIsValidResponse(string cep, string streetAddress, string city, string uf)
        {
            var response = Response.Create();

            if (string.IsNullOrEmpty(cep))
                response.WithBusinessError(nameof(cep), $"O campo {nameof(cep)} é obrigatório.");

            if (string.IsNullOrEmpty(streetAddress))
                response.WithBusinessError(nameof(streetAddress), $"O campo {nameof(streetAddress)} é obrigatório.");

            if (string.IsNullOrEmpty(city))
                response.WithBusinessError(nameof(city), $"O campo {nameof(city)} é obrigatório.");

            if (string.IsNullOrEmpty(uf))
                response.WithBusinessError(nameof(uf), $"O campo {nameof(uf)} é obrigatório.");

            return response;
        }


        public Response Update(string cep, string streetAddress, string city, string uf, string complement = "")
        {
            var response = Response.Create();

            var addressIsValidToUpdate = AddressIsValidResponse(cep, streetAddress, city, uf);

            if (addressIsValidToUpdate.HasError)
                return response.WithMessages(addressIsValidToUpdate.Messages);

            Cep = cep;
            StreetAddress = streetAddress;
            City = city;
            UF = uf;
            Complement = complement;

            return response;
        }

        public Response SetLocation(double longitude, double latitude)
        {
            var response = Response.Create();

            var coordinatesAreValidResponse = CoordinatesAreValidResponse(longitude, latitude);

            if (coordinatesAreValidResponse.HasError)
                return coordinatesAreValidResponse;

            Location = new double[] { longitude, latitude };

            return response;
        }

        Response CoordinatesAreValidResponse(double longitude, double latitude)
        {
            var response = Response.Create();

            if (!LatitudeIsValid(latitude))
                response.WithBusinessError(nameof(latitude), "Coordenadas de latitude devem estar entre -90 e 90");

            if (!LongitudeIsValid(longitude))
                response.WithBusinessError(nameof(longitude), "Coordenadas de longitude devem estar entre -180 e 180");

            return response;
        }

        bool LatitudeIsValid(double latitude) => latitude <= 90 && latitude >= -90;

        bool LongitudeIsValid(double longitude) => longitude <= 180 && longitude >= -180;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            else if (!(obj is Address))
                return false;

            return StreetAddress == ((Address)obj).StreetAddress
                && (Complement == ((Address)obj).Complement)
                && (Cep == ((Address)obj).Cep)
                && (City == ((Address)obj).City)
                && (UF == ((Address)obj).UF);
        }

        public override int GetHashCode()
            => StreetAddress.GetHashCode()
                ^ Complement.GetHashCode()
                ^ Cep.GetHashCode()
                ^ City.GetHashCode()
                ^ UF.GetHashCode();

        #endregion

        #region Convertion Operators

        public static implicit operator Address(Maybe<Address> entity) => entity.Value;

        public static implicit operator Address(Response<Address> entity) => entity.Data;

        #endregion
    }
}
