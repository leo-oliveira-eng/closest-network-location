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
    public class ImportAsyncUnitTests : GasStationServiceUnitTests
    {
        private ImportGasStationRequestMessage _requestMessage;

        protected override void BeforeCreateService()
        {
            _requestMessage = ImportGasStationRequestMessageFake();
        }

        [TestMethod]
        public async Task ImportAsync_ShoultReturnResponseWithBusinessError_RequestMessageIsNull()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>())).Verifiable();
            _gasStationRepository.Setup(_ => _.AddAsync(It.IsAny<Model.GasStation>())).Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>())).Verifiable();

            var response = await GasStationService.ImportAsync(requestMessage: null);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnResponseWithBusinessError_FailToCreateGasStation()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .ReturnsAsync(Response.Create().WithBusinessError("Fail trying to create GasStation"))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.AddAsync(It.IsAny<Model.GasStation>())).Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Maybe<Model.GasStation>.Create())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>())).Verifiable();

            var response = await GasStationService.ImportAsync(_requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
            _gasStationFactory.Verify();
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnResponseWithBusinessError_FailToUpdateGasStation_AddressIsInvalid()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.AddAsync(It.IsAny<Model.GasStation>())).Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>())).Verifiable();

            var gasStationFake = GasStationRequestMessageFake(uf: string.Empty);

            var response = await GasStationService.ImportAsync(ImportGasStationRequestMessageFake(new List<GasStationRequestMessage> { gasStationFake }));

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnResponseWithCriticalError_FailToUpdateGasStation_GeolocationServiceReturnsError()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.AddAsync(It.IsAny<Model.GasStation>())).Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(Response<LocationDto>.Create().WithCriticalError("something went wrong"))
                .Verifiable();

            var response = await GasStationService.ImportAsync(_requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.CriticalError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify();
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnResponseWithBusinessError_FailToUpdateGasStation_GeolocationServiceReturnsInvalidCordinates()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.AddAsync(It.IsAny<Model.GasStation>())).Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(LocationDtoFake(-500, -25))
                .Verifiable();

            var response = await GasStationService.ImportAsync(_requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify();
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnResponseWithBusinessError_FailToUpdateGasStation_NameIsInvalid()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.UpdadeAsync(It.IsAny<Model.GasStation>()))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(LocationDtoFake())
                .Verifiable();

            var gasStationFake = GasStationRequestMessageFake(name: string.Empty);

            var response = await GasStationService.ImportAsync(ImportGasStationRequestMessageFake(new List<GasStationRequestMessage> { gasStationFake }));

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify();
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnResponseWithCriticalError_FailToUpdateGasStation_RepositoryReturnsError()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .Verifiable();
            _updateResult.Setup(_ => _.IsAcknowledged).Returns(false);
            _gasStationRepository.Setup(_ => _.UpdadeAsync(It.IsAny<Model.GasStation>()))
                .ReturnsAsync(_updateResult.Object)
                .Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(LocationDtoFake())
                .Verifiable();

            var response = await GasStationService.ImportAsync(_requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.CriticalError));
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify();
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnSuccessResponse_UpdateIsSuccess()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .Verifiable();
            _updateResult.Setup(_ => _.IsAcknowledged).Returns(true);
            _gasStationRepository.Setup(_ => _.UpdadeAsync(It.IsAny<Model.GasStation>()))
                .ReturnsAsync(_updateResult.Object)
                .Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(LocationDtoFake())
                .Verifiable();

            var response = await GasStationService.ImportAsync(_requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _geolocationExternalService.Verify();
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportAsync_ShouldReturnSuccessResponse_CreateIsSuccess()
        {
            _gasStationFactory.Setup(_ => _.CreateAsync(It.IsAny<GasStationDto>()))
                .ReturnsAsync(Response.Create())
                .Verifiable();
            _gasStationRepository.Setup(_ => _.UpdadeAsync(It.IsAny<Model.GasStation>()))
                .Verifiable();
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Maybe<Model.GasStation>.Create())
                .Verifiable();
            _geolocationExternalService.Setup(_ => _.GetGeolocationAsync(It.IsAny<string>()))
                .Verifiable();

            var response = await GasStationService.ImportAsync(_requestMessage);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            _gasStationRepository.Verify(_ => _.UpdadeAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _gasStationRepository.Verify(_ => _.AddAsync(It.IsAny<Model.GasStation>()), Times.Never);
            _geolocationExternalService.Verify(_ => _.GetGeolocationAsync(It.IsAny<string>()), Times.Never);
            _gasStationFactory.Verify(_ => _.CreateAsync(It.IsAny<GasStationDto>()), Times.Once);
        }
    }
}
