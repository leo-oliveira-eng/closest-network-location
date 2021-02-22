using FluentAssertions;
using Messages.Core;
using Messages.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Model = Closest.Network.Location.API.Models;
using Services = Closest.Network.Location.API.Services;

namespace Closest.Network.Location.UnitTests.GasStationService
{
    [TestClass, TestCategory(nameof(Services.GasStationService))]
    public class RemoveAsyncUnitTests : GasStationServiceUnitTests
    {
        [TestMethod]
        public async Task RemoveAsync_ShoultReturnResponseWithBusinessError_ExternalIdIsEmpty()
        {
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()));
            _gasStationRepository.Setup(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()));

            var response = await GasStationService.RemoveAsync(externalId: string.Empty);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Never);
            _gasStationRepository.Verify(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()), Times.Never);
        }

        [TestMethod]
        public async Task RemoveAsync_ShoultReturnResponseWithBusinessError_GasStationNotFound()
        {
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(Maybe<Model.GasStation>.Create());
            _gasStationRepository.Setup(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()));

            var response = await GasStationService.RemoveAsync(externalId: "Any ExternalId");

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.BusinessError));
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _gasStationRepository.Verify(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()), Times.Never);
        }

        [TestMethod]
        public async Task RemoveAsync_ShoultReturnResponseWithCriticalError_RepositoryFailedToDeleteGasStation()
        {
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake());
            _updateResult.Setup(_ => _.IsAcknowledged).Returns(false);
            _gasStationRepository.Setup(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()))
                .ReturnsAsync(_updateResult.Object);

            var response = await GasStationService.RemoveAsync(externalId: "Any ExternalId");

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Type.Equals(MessageType.CriticalError));
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _gasStationRepository.Verify(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveAsync_ShoultReturnSuccessResponse_GasStationSoftDeleted()
        {
            _gasStationRepository.Setup(_ => _.FindByExternalIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GasStationFake());
            _updateResult.Setup(_ => _.IsAcknowledged).Returns(true);
            _gasStationRepository.Setup(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()))
                .ReturnsAsync(_updateResult.Object);

            var response = await GasStationService.RemoveAsync(externalId: "Any ExternalId");

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            _gasStationRepository.Verify(_ => _.FindByExternalIdAsync(It.IsAny<string>()), Times.Once);
            _gasStationRepository.Verify(_ => _.DeleteAsync(It.IsAny<Model.GasStation>()), Times.Once);
        }
    }
}