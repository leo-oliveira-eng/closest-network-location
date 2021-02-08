using Closest.Network.Location.UnitTests.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.GasStation
{
    [TestClass, TestCategory(nameof(Model.GasStation))]
    public class CreateUnitTests : BaseMock
    {
        [TestMethod]
        public void CreateGasStation_ShouldCreateWithValidParameters()
        {
            var gasStationDto = GasStationDtoFake();
            var address = AddressFake();

            var response = Model.GasStation.Create(gasStationDto, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            response.Data.HasValue.Should().BeTrue();
            response.Data.Value.Should().BeOfType(typeof(Model.GasStation));
            response.Data.Value.Name.Should().Be(gasStationDto.Name);
        }

        [TestMethod]
        public void CreateGasStation_ShouldCreateWithValidParameters_SiteUrlIsEmpty()
        {
            var gasStationDto = GasStationDtoFake(siteUrl: string.Empty);
            var address = AddressFake();

            var response = Model.GasStation.Create(gasStationDto, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            response.Data.HasValue.Should().BeTrue();
            response.Data.Value.Should().BeOfType(typeof(Model.GasStation));
            response.Data.Value.Name.Should().Be(gasStationDto.Name);
        }

        [TestMethod]
        public void CreateGasStation_ShouldReturnBusinessError_ExternalIdIsEmpty()
        {
            var gasStationDto = GasStationDtoFake(externalID: string.Empty);
            var address = AddressFake();

            var response = Model.GasStation.Create(gasStationDto, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("ExternalId"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateGasStation_ShouldReturnBusinessError_NameIsEmpty()
        {
            var gasStationDto = GasStationDtoFake(name: string.Empty);
            var address = AddressFake();

            var response = Model.GasStation.Create(gasStationDto, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("Name"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateGasStation_ShouldReturnBusinessError_PhoneNumberIsEmpty()
        {
            var gasStationDto = GasStationDtoFake(phoneNumber: string.Empty);
            var address = AddressFake();

            var response = Model.GasStation.Create(gasStationDto, address);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("PhoneNumber"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }

        [TestMethod]
        public void CreateGasStation_ShouldReturnBusinessError_AddressIsNull()
        {
            var gasStationDto = GasStationDtoFake();

            var response = Model.GasStation.Create(gasStationDto, null);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("address"));
            response.Data.HasValue.Should().BeFalse();
            response.Data.Value.Should().BeNull();
        }
    }
}
