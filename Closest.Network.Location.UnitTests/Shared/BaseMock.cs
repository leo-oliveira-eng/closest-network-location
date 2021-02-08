using Closest.Network.Location.API.Services.Dtos;
using FizzWare.NBuilder;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.Shared
{
    public class BaseMock
    {
        public Model.Address AddressFake(string cep = null, string streetAddress = null, string city = null, string uf = null, string complement = null)
            => Builder<Model.Address>.CreateNew()
                .With(x => x.Cep, cep ?? "11111-000")
                .With(x => x.StreetAddress, streetAddress ?? "Another street")
                .With(x => x.City, city ?? "Another City")
                .With(x => x.UF, uf ?? "UF")
                .With(x => x.Complement, complement)
                .Build();

        public GasStationDto GasStationDtoFake(string externalID = null, string name = null, string phoneNumber = null, string siteUrl = null, AddressDto address = null)
        {
            var addressFake = address ?? Builder<AddressDto>.CreateNew().Build();

            return Builder<GasStationDto>.CreateNew()
                .With(x => x.ExternalId, externalID ?? "Any ID")
                .With(x => x.Name, name ?? "Any Name")
                .With(x => x.PhoneNumber, phoneNumber ?? "1234-1234")
                .With(x => x.SiteUrl, siteUrl ?? "any@nothing.com")
                .With(x => x.Address, addressFake)
                .Build();
        }

        public Model.GasStation GasStationFake(string id = null, string externalId = null, string name = null, string phoneNumber = null, string siteUrl = null, Model.Address address = null)
        {
            var addressFake = address ?? Builder<Model.Address>.CreateNew().Build();

            return Builder<Model.GasStation>.CreateNew()
                .With(x => x.Id, id ?? "some id")
                .With(x => x.ExternalId, externalId ?? "any external id")
                .With(x => x.Name, name ?? "Any Name")
                .With(x => x.PhoneNumber, phoneNumber ?? "0987654321")
                .With(x => x.SiteUrl, siteUrl ?? "nothing@none.com")
                .With(x => x.Address, addressFake)
                .Build();
        }

        public LocationDto LocationDtoFake(double? latitude = null, double? longitude = null)
            => Builder<LocationDto>.CreateNew()
                .With(x => x.Longitude, longitude ?? -43)
                .With(x => x.Latitude, latitude ?? -22)
                .Build();

        public AddressDto AddressDtoFake(string cep = null, string streetAddress = null, string city = null, string uf = null, string complement = null)
            => Builder<AddressDto>.CreateNew()
                .With(x => x.Cep, cep ?? "11111-000")
                .With(x => x.StreetAddress, streetAddress ?? "Another street")
                .With(x => x.City, city ?? "Another City")
                .With(x => x.UF, uf ?? "UF")
                .With(x => x.Complement, complement)
                .Build();
    }
}
