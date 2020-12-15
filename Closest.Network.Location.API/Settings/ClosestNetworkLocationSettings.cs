using Closest.Network.Location.API.Settings.Contracts;

namespace Closest.Network.Location.API.Settings
{
    public class ClosestNetworkLocationSettings : IClosestNetworkLocationSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public GeocoddingSettings GeocoddingSettings { get; set; }

        public string GeocodeApiBaseUrl { get; set; }
    }
}
