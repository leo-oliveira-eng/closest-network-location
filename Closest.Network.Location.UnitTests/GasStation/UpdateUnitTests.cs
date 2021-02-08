using Closest.Network.Location.API.Services.Dtos;
using Closest.Network.Location.UnitTests.Shared;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.GasStation
{
    [TestClass, TestCategory(nameof(Model.GasStation))]
    public class UpdateUnitTests : BaseMock
    {
        [TestMethod]
        public void UpdateGasStation_WithValidParameters_ShouldReturnAnEmptyResponse()
        {
            var gasStation = GasStationFake();
            var gasStatioUpdated = GasStationDtoFake();
            var address = AddressFake();

            var response = gasStation.Update(gasStatioUpdated, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            gasStation.Address.Cep.Should().Be(address.Cep);
        }

        [TestMethod]
        public void UpdateGasStation_SiteUrlIsEmpty_ShouldReturnAnEmptyResponse()
        {
            var gasStation = GasStationFake();
            var gasStatioUpdated = GasStationDtoFake(siteUrl: string.Empty);
            var address = AddressFake();

            var response = gasStation.Update(gasStatioUpdated, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            gasStation.Address.Cep.Should().Be(address.Cep);
        }

        [TestMethod]
        public void UpdateGasStation_ShouldReturnBusinessError_ExternalIdIsEmpty()
        {
            var gasStation = GasStationFake();
            var gasStatioUpdated = GasStationDtoFake(externalID: string.Empty);
            var address = AddressFake();

            var response = gasStation.Update(gasStatioUpdated, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("ExternalId"));
        }

        [TestMethod]
        public void UpdateGasStation_ShouldReturnBusinessError_NameIsEmpty()
        {
            var gasStation = GasStationFake();
            var gasStatioUpdated = GasStationDtoFake(name: string.Empty);
            var address = AddressFake();

            var response = gasStation.Update(gasStatioUpdated, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("Name"));
        }

        [TestMethod]
        public void UpdateGasStation_ShouldReturnBusinessError_PhoneNumberIsEmpty()
        {
            var gasStation = GasStationFake();
            var gasStatioUpdated = GasStationDtoFake(phoneNumber: string.Empty);
            var address = AddressFake();

            var response = gasStation.Update(gasStatioUpdated, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("PhoneNumber"));
        }

        [TestMethod]
        public void UpdateGasStation_ShouldReturnBusinessError_AddressIsNull()
        {
            var gasStation = GasStationFake();
            var gasStationUpdated = Builder<GasStationDto>.CreateNew().Build();

            var response = gasStation.Update(gasStationUpdated, null);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("address"));
        }
    }
}
