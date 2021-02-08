using Closest.Network.Location.UnitTests.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.Address
{
    [TestClass, TestCategory(nameof(Model.Address))]
    public class EqualsUnitTests : BaseMock
    {
        [TestMethod]
        public void AddressEquals_ShouldReturnTrue_WithSimilarParameters()
        {
            var addressFake1 = AddressFake(cep: "12345-123", streetAddress: "Any street", city: "Toxixity", uf: "ANY");
            var addressFake2 = AddressFake(cep: "12345-123", streetAddress: "Any street", city: "Toxixity", uf: "ANY", complement: "Another block");

            var response = addressFake1.Equals(addressFake2);

            response.Should().BeTrue();
        }

        [TestMethod]
        public void AddressEquals_ShouldReturnFalse_ObjectIsNull()
        {
            var addressFake1 = AddressFake(cep: "12345-123", streetAddress: "Any street", city: "Toxixity", uf: "ANY");
            Model.Address addressFake2 = null;

            var response = addressFake1.Equals(addressFake2);

            response.Should().BeFalse();
        }

        [TestMethod]
        public void AddressEquals_ShouldReturnFalse_DiferentType()
        {
            var addressFake1 = AddressFake(cep: "12345-123", streetAddress: "Any street", city: "Toxixity", uf: "ANY");
            var addressFake2 = AddressDtoFake(cep: "12345-123", streetAddress: "Any street", city: "Toxixity", uf: "ANY", complement: "Another block");

            var response = addressFake1.Equals(addressFake2);

            response.Should().BeFalse();
        }

        [TestMethod]
        public void AddressEquals_ShouldReturnFalse_DiferentStreetAddress()
        {
            var addressFake1 = AddressFake(cep: "12345-123", streetAddress: "Any street", city: "Toxixity", uf: "ANY");
            var addressFake2 = AddressFake(cep: "12345-123", streetAddress: "Another street", city: "Toxixity", uf: "ANY", complement: "Another block");

            var response = addressFake1.Equals(addressFake2);

            response.Should().BeFalse();
        }
    }
}