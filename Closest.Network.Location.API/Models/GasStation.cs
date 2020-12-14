using System;

namespace Closest.Network.Location.API.Models
{
    public class GasStation
    {
        #region Properties

        public string Id { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public DateTime LastUpdate { get; private set; } = DateTime.Now;

        public DateTime? DeletedAt { get; private set; }

        public bool Deleted => DeletedAt.HasValue;

        public string ExternalId { get; private set; }

        public string Name { get; private set; }

        public string PhoneNumber { get; private set; }

        public string SiteUrl { get; private set; }

        public Address Address { get; private set; }

        #endregion

        #region Constructors

        [Obsolete("Created only for EF", true)]
        public GasStation() { }

        public GasStation(string externalId, string name, string phoneNumber, string siteUrl, Address address)
        {
            ExternalId = externalId;
            Name = name;
            PhoneNumber = phoneNumber;
            SiteUrl = siteUrl;
            Address = address;
        }

        #endregion
    }
}
