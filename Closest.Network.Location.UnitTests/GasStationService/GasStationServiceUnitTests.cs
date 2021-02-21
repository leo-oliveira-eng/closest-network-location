using Closest.Network.Location.API.Data.Contracts;
using Closest.Network.Location.API.Factories.Contracts;
using Closest.Network.Location.API.Services.ExternalServices.Contracts;
using Closest.Network.Location.UnitTests.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using Services = Closest.Network.Location.API.Services;

namespace Closest.Network.Location.UnitTests.GasStationService
{
    [TestClass, TestCategory(nameof(Services.GasStationService))]
    public class GasStationServiceUnitTests : BaseMock
    {
        #region Fields

        protected readonly Mock<IGasStationRepository> _gasStationRepository = new Mock<IGasStationRepository>();

        protected readonly Mock<IGeolocationExternalService> _geolocationExternalService = new Mock<IGeolocationExternalService>();

        protected readonly Mock<IGasStationFactory> _gasStationFactory = new Mock<IGasStationFactory>();

        protected readonly Mock<UpdateResult> _updateResult = new Mock<UpdateResult>();

        #endregion

        #region Properties

        protected Services.GasStationService GasStationService { get; set; }

        #endregion

        #region Methods

        [TestInitialize]
        public void TestInitialize()
        {
            BeforeCreateService();

            GasStationService = new Services.GasStationService(_gasStationRepository.Object
                , _geolocationExternalService.Object
                , _gasStationFactory.Object);
        }

        protected virtual void BeforeCreateService() { }

        #endregion
    }
}