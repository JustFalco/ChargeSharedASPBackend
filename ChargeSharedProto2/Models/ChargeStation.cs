using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ChargeSharedProto2.Models
{
    public class ChargeStation
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public UserAdres Adres { get; set; }
        
        [Required]
        [JsonIgnore]
        public string OwnerId { get; set; }
        
        [JsonIgnore]
        [Required]
        public ApplicationUser Owner { get; set; }
        public ChargerType ChargerType { get; set; }
        public double PricePerHour { get; set; }
        public bool QuickCharge { get; set; }
    }

    public enum ChargerType
    {
        J1772 = 0,
        CCS_Combo_1 = 1,
        Type_2 = 2,
        CCS_Combo_2 = 3,
        CHAdeMO = 4,
        Tesla = 5,
        Wall_Outlet = 6
    }
}
