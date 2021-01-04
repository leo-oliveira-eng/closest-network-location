using Closest.Network.Location.UnitTests.Shared;
using FluentAssertions;
using Messages.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.Address
{
    [TestClass, TestCategory(nameof(Model.Address))]
    public class UpdateUnitTests : BaseMock
    {
        readonly string _cep = "12345-678";
        readonly string _streetAddress = "Any Address";
        readonly string _complement = "Anything";
        readonly string _city = "Any City";
        readonly string _uf = "ANY";

        [TestMethod]
        public void UpdateAddress_WithValidParameters_ShouldReturnAnEmptyResponse()
        {
            var address = AddressFake();

            var response = address.Update(_cep, _streetAddress, _city, _uf, _complement);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            address.UF.Should().Be(_uf);
        }

        [TestMethod]
        public void UpdateAddress_ComplementIsEmpty_ShouldReturnAnEmptyResponse()
        {
            var address = AddressFake();

            var response = address.Update(_cep, _streetAddress, _city, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            address.UF.Should().Be(_uf);
            address.Complement.Should().BeEmpty();
        }

        [TestMethod]
        public void UpdateAddress_ShouldReturnBusinessError_CepIsEmpty()
        {
            var address = AddressFake();

            var response = address.Update(string.Empty, _streetAddress, _city, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("cep"));
        }

        [TestMethod]
        public void UpdateAddress_ShouldReturnBusinessError_StreetIsEmpty()
        {
            var address = AddressFake();

            var response = address.Update(_cep, string.Empty, _city, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("streetAddress"));
        }

        [TestMethod]
        public void UpdateAddress_ShouldReturnBusinessError_CityIsEmpty()
        {
            var address = AddressFake();

            var response = address.Update(_cep, _streetAddress, string.Empty, _uf, string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("city"));
        }

        [TestMethod]
        public void UpdateAddress_ShouldReturnBusinessError_StateIsEmpty()
        {
            var address = AddressFake();

            var response = address.Update(_cep, _streetAddress, _city, string.Empty, _complement);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("uf"));
        }

        [TestMethod]
        public void UpdateAddress_ShouldReturnBusinessError_AllArgumentsAreNullOrEmpty()
        {
            var address = AddressFake();

            var response = address.Update(null, null, string.Empty, string.Empty, null);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(4);
            response.Messages.All(m => m.Type.Equals(MessageType.BusinessError));
            response.Messages.Should().Contain(message => message.Property.Equals("uf"));
        }
    }
}
