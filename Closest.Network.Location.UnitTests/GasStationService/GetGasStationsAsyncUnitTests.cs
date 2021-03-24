using Closest.Network.Location.API.Messages.RequestMessages;
using Closest.Network.Location.API.Services.Dtos;
using FluentAssertions;
using Messages.Core;
using Messages.Core.Enums;
using Messages.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = Closest.Network.Location.API.Models;
using Services = Closest.Network.Location.API.Services;

namespace Closest.Network.Location.UnitTests.GasStationService
{
    [TestClass, TestCategory(nameof(Services.GasStationService))]
    public class GetGasStationsAsyncUnitTests : GasStationServiceUnitTests
    {
        [TestMethod]
        public async Task GetGasStationsAsync_ShoultReturnResponseWithBusinessError_RequestMessageIsNull()
        {
            _gasStationRepository.Setup(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()));
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()));

            var response = await GasStationService.GetGasStationsAsync(null);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()), Times.Never);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetGasStationsAsync_ShoultReturnResponseWithBusinessError_InvalidSearchData()
        {
            _gasStationRepository.Setup(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()));
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()));

            var requestMessage = new GetGasStationRequestMessage
            {
                Address = string.Empty,
                Latitude = string.Empty,
                Longitude = "0"
            };

            var response = await GasStationService.GetGasStationsAsync(requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()), Times.Never);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetGasStationsAsync_ShoultReturnResponseWithBusinessError_InvalidCoordinates()
        {
            _gasStationRepository.Setup(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()));
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()));

            var requestMessage = new GetGasStationRequestMessage
            {
                Latitude = "1000",
                Longitude = "-500"
            };

            var response = await GasStationService.GetGasStationsAsync(requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(2);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            response.Messages.Should().Contain(message => message.Property.Equals("latitude"));
            _gasStationRepository.Verify(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()), Times.Never);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task GetGasStationsAsync_ShoultReturnResponseWithBusinessError_InvalidAddress()
        {
            _gasStationRepository.Setup(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()));
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create().WithBusinessError("Any error"))
                .Verifiable();

            var requestMessage = new GetGasStationRequestMessage
            {
                Address = "Any address"
            };

            var response = await GasStationService.GetGasStationsAsync(requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()), Times.Never);
            _geolocationExternalService.Verify();
        }

        [TestMethod]
        public async Task GetGasStationsAsync_ShoultReturnResponseWithBusinessError_GasStationRepositoryReturnError()
        {
            _gasStationRepository.Setup(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()))
                .ReturnsAsync(Response<List<Model.GasStation>>.Create().WithBusinessError("AnyError"))
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create(new LocationDto()))
                .Verifiable();

            var requestMessage = new GetGasStationRequestMessage
            {
                Address = "Any address"
            };

            var response = await GasStationService.GetGasStationsAsync(requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify();
            _geolocationExternalService.Verify();
        }

        [TestMethod]
        public async Task GetGasStationsAsync_ShouldReturnSuccess()
        {
            _gasStationRepository.Setup(_ => _.GetGasStationsByLocationAsync(It.IsAny<GetLocationDto>()))
                .ReturnsAsync(Response<List<Model.GasStation>>.Create(new List<Model.GasStation> { GasStationFake() }))
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create(new LocationDto()))
                .Verifiable();

            var requestMessage = new GetGasStationRequestMessage
            {
                Address = "Any address"
            };

            var response = await GasStationService.GetGasStationsAsync(requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            _gasStationRepository.Verify();
            _geolocationExternalService.Verify();
        }
    }
}
