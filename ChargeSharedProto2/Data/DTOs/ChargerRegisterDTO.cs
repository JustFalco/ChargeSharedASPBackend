using ChargeSharedProto2.Models;
using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace ChargeSharedProto2.Data.DTOs
{
    public class ChargerRegisterDTO
    {
        [Required]
        public string chargerName { get; set; }

        [Required] 
        public int chargerType { get; set; }

        [Required]
        [Range(0.000000001, Double.MaxValue, ErrorMessage = "Price cannot be 0!")]
        public double pricePerHour { get; set; }

        public bool quickcharge { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string HouseNumber { get; set; }
        public string HouseAddition { get; set; }


        public NewAdressDTO getAdres()
        {
            return new NewAdressDTO
            {
                Street = this.Street,
                City = this.City,
                PostalCode = this.PostalCode,
                Country = this.Country,
                Province = this.Province,
                HouseAddition = this.HouseAddition,
                HouseNumber = this.HouseNumber,
            };
        }
    }
}

