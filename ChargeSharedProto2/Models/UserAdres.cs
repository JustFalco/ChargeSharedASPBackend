namespace ChargeSharedProto2.Models
{
    public class UserAdres
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string HouseNumber { get; set; }
        public string HouseAddition { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
