namespace Closest.Network.Location.API.Services.Dtos
{
    public class AddressDto
    {
        public string Cep { get; set; }

        public string StreetAddress { get; set; }

        public string Complement { get; set; }

        public string City { get; set; }

        public string UF { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
