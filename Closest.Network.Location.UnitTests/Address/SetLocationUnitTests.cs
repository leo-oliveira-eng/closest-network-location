using Closest.Network.Location.UnitTests.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model = Closest.Network.Location.API.Models;

namespace Closest.Network.Location.UnitTests.Address
{
    [TestClass, TestCategory(nameof(Model.Address))]
    public class SetLocationUnitTests : BaseMock
    {
        readonly double _latitude = 20;
        readonly double _longitude = -20;
        Model.Address _address;

        [TestInitialize]
        public void TestInitialize()
        {
            _address = AddressFake();
        }

        [TestMethod]
        public void SetLocation_WithValidParams_ShouldReturnAnEmptyResponse()
        {
            var response = _address.SetLocation(_longitude, _latitude);

            response.Should().NotBeNull();
            response.HasError.Should().BeFalse();
            response.Messages.Should().BeEmpty();
            _address.Location[0].Should().Equals(_longitude);
            _address.Location[1].Should().Equals(_latitude);
        }

        [TestMethod]
        public void SetLocation_ShouldReturnError_LongitudeLessThanLowerLimit()
        {
            var response = _address.SetLocation(-300, _latitude);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("longitude"));
        }

        [TestMethod]
        public void SetLocation_ShouldReturnError_LongitudeGreaterThanLowerLimit()
        {
            var response = _address.SetLocation(300, _latitude);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("longitude"));
        }

        [TestMethod]
        public void SetLocation_ShouldReturnError_LatitudeLessThanLowerLimit()
        {
            var response = _address.SetLocation(_longitude, -200);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("latitude"));
        }

        [TestMethod]
        public void SetLocation_ShouldReturnError_LatitudeGreaterThanLowerLimit()
        {
            var response = _address.SetLocation(_longitude, 200);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(1);
            response.Messages.Should().Contain(message => message.Property.Equals("latitude"));
        }

        [TestMethod]
        public void SetLocation_ShouldReturnError_BothLatitudeAndLongitudeAreInvalid()
        {
            var response = _address.SetLocation(300, 200);

            response.Should().NotBeNull();
            response.HasError.Should().BeTrue();
            response.Messages.Should().HaveCount(2);
            response.Messages.Should().Contain(message => message.Property.Equals("latitude"));
            response.Messages.Should().Contain(message => message.Property.Equals("longitude"));
        }
    }
}
