using FluentAssertions;
using Messages.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.Address
{
    [TestClass, TestCategory(nameof(Model.Address))]
    public class CreateUnitTests
    {
        readonly string _cep = "12345-678";
        readonly string _streetAddress = "Any Address";
        readonly string _complement = "Anything";
        readonly string _city = "Any City";
        readonly string _uf = "ANY";

        [TestMethod]
        public void CreateAddress_ShouldCreateWithValidParameters()
        {
            var response = Model.Address.Create(_cep, _streetAddress, _city, _uf, _complement);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            response.Data.HasValue.Should().BeTrue();
            response.Data.Value.Should().BeOfType(typeof(Model.Address));
            response.Data.Value.Cep.Should().Be(_cep);
        }

        [TestMethod]
        public void CreateAddress_ShouldCreateWithValidParameters_ComplementIsEmpty()
        {
            var response = Model.Address.Create(_cep, _streetAddress, _city, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            response.Data.HasValue.Should().BeTrue();
            response.Data.Value.Should().BeOfType(typeof(Model.Address));
            response.Data.Value.Cep.Should().Be(_cep);
        }

        [TestMethod]
        public void CreateAddress_ShouldReturnBusinessError_CepIsEmpty()
        {
            var response = Model.Address.Create(string.Empty, _streetAddress, _city, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("cep"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateAddress_ShouldReturnBusinessError_StreetIsEmpty()
        {
            var response = Model.Address.Create(_cep, string.Empty, _city, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("streetAddress"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateAddress_ShouldReturnBusinessError_CityIsEmpty()
        {
            var response = Model.Address.Create(_cep, _streetAddress, string.Empty, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("city"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateAddress_ShouldReturnBusinessError_StateIsEmpty()
        {
            var response = Model.Address.Create(_cep, _streetAddress, _city, string.Empty, _complement);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("uf"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateAddress_ShouldReturnBusinessError_AllArgumentsAreNullOrEmpty()
        {
            var response = Model.Address.Create(null, null, string.Empty, string.Empty, null);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(4);
            response.Messages.All(m => m.Type.Equals(MessageType.BusinessError));
            response.Messages.Should().Contain(message => message.Property.Equals("uf"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }
    }
}
