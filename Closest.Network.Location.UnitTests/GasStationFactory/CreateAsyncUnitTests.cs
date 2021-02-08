using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Services.Dtos;
using Closest.Network.Location.API.Services.ExternalServices.Contracts;
using Closest.Network.Location.UnitTests.Shared;
using FluentAssertions;
using Messages.Core;
using Messages.Core.Enums;
using Messages.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Factories = Closest.Network.Location.API.Factories;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.GasStationFactory
{
    [TestClass, TestCategory(nameof(Factories.GasStationFactory))]
    public class CreateAsyncUnitTests : BaseMock
    {
        #region Fields

        readonly Mock<IGeolocationExternalService> _geolocationExternalService = new Mock<IGeolocationExternalService>();
        readonly Mock<IGasStationRepository> _gasStationRepository = new Mock<IGasStationRepository>();

        #endregion

        #region Properties

        Factories.GasStationFactory GasStationFactory { get; set; }

        #endregion

        #region Unit tests

        [TestInitialize]
        public void TestInitialize()
        {
            GasStationFactory = new Factories.GasStationFactory(_geolocationExternalService.Object, _gasStationRepository.Object);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnAValidResponse()
        {
            var gasStation = GasStationFake();

            _geolocationExternalService.Setup(x => x.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create(LocationDtoFake()))
                .Verifiable();

            _gasStationRepository.Setup(x => x.AddAsync(It.IsAny<Model.GasStation>()))
                .ReturnsAsync(Response<Model.GasStation>.Create(gasStation))
                .Verifiable();

            var response = await GasStationFactory.CreateAsync(GasStationDtoFake());

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            _gasStationRepository.Verify();
            _geolocationExternalService.Verify();
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnError_InvalidStreetAddress()
        {
            var address = AddressDtoFake(streetAddress: string.Empty);

            _geolocationExternalService.Setup(x => x.GetGeolocationAsync(It.IsAny<string>()))
                .Verifiable();

            _gasStationRepository.Setup(x => x.AddAsync(It.IsAny<Model.GasStation>()))
                .Verifiable();
                
            var response = await GasStationFactory.CreateAsync(GasStationDtoFake(address: address));

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("streetAddress"));
            _gasStationRepository.Verify(x => x.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _geolocationExternalService.Verify(x => x.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnError_GeocodingExternalServiceReturnsError()
        {
            _geolocationExternalService.Setup(x => x.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create().WithBusinessError("Endereço não foi encontrado"))
                .Verifiable();

            _gasStationRepository.Setup(x => x.AddAsync(It.IsAny<Model.GasStation>()))
                .Verifiable();

            var response = await GasStationFactory.CreateAsync(GasStationDtoFake());

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Text.Equals("Endereço não foi encontrado"));
            _gasStationRepository.Verify(x => x.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _geolocationExternalService.Verify();
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnError_CoordinatesAreInvalid()
        {
            _geolocationExternalService.Setup(x => x.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create(LocationDtoFake(latitude: -300, longitude: -200)))
                .Verifiable();

            _gasStationRepository.Setup(x => x.AddAsync(It.IsAny<Model.GasStation>()))
                .Verifiable();

            var response = await GasStationFactory.CreateAsync(GasStationDtoFake());

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(2);
            response.Messages.Should().Contain(message => message.Property.Equals("latitude"));
            _gasStationRepository.Verify(x => x.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _geolocationExternalService.Verify();
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnError_GasStationNameIsInvalid()
        {
            _geolocationExternalService.Setup(x => x.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create(LocationDtoFake()))
                .Verifiable();

            _gasStationRepository.Setup(x => x.AddAsync(It.IsAny<Model.GasStation>()))
                .Verifiable();

            var response = await GasStationFactory.CreateAsync(GasStationDtoFake(name: string.Empty));

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("Name"));
            _gasStationRepository.Verify(x => x.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _geolocationExternalService.Verify();
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnError_GasStationRepositoryRetunsError()
        {
            _geolocationExternalService.Setup(x => x.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create(LocationDtoFake()))
                .Verifiable();

            _gasStationRepository.Setup(x => x.AddAsync(It.IsAny<Model.GasStation>()))
                .ReturnsAsync(Response<Model.GasStation>.Create().WithCriticalError("Something failed"))
                .Verifiable();

            var response = await GasStationFactory.CreateAsync(GasStationDtoFake());

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.CriticalError));
            _gasStationRepository.Verify();
            _geolocationExternalService.Verify();
        }

        #endregion
    }
}