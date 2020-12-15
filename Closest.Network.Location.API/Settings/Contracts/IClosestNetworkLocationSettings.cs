namespace Closest.Network.Location.API.Settings.Contracts
{
    public interface IClosestNetworkLocationSettings
    {
        string CollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        GeocoddingSettings GeocoddingSettings { get; set; }

        string GeocodeApiBaseUrl { get; set; }
    }
}
