using Microsoft.AspNetCore.Identity;

namespace ChargeSharedProto2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public bool IsValidUser { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;

        public int Age
        {
            get
            {
                //https://stackoverflow.com/a/1595311/14119518
                int age = DateTime.Now.Year - DateOfBirth.Year;

                // For leap years we need this
                if (DateOfBirth > DateTime.Now.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}