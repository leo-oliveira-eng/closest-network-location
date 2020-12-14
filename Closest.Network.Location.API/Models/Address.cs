using System;

namespace Closest.Network.Location.API.Models
{
    public class Address
    {
        #region Properties

        public string Cep { get; private set; }

        public string Street { get; private set; }

        public string Number { get; private set; }

        public string Complement { get; private set; }

        public string City { get; private set; }

        public string UF { get; private set; }

        public double[] Location { get; set; }

        #endregion

        #region Constructors

        [Obsolete("Created only for EF", true)]
        public Address() { }

        public Address(string cep, string street, string number, string complement, string city, string uF)
        {
            Cep = cep;
            Street = street;
            Number = number;
            Complement = complement;
            City = city;
            UF = uF;
        }

        #endregion
    }
}
