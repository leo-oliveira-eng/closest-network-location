namespace Closest.Network.Location.API.Settings
{
    public class GeocoddingSettings
    {
        public string GeocodeApiBaseUrl { get; set; }

        public string GeocodeApiBaseSetting { get; set; }

        public string GeocodeApiKey { get; set; }

        public int BaseRadius { get; set; }

        public int MaxRadius { get; set; }
    }
}
