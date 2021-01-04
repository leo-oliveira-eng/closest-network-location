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
    }
}
