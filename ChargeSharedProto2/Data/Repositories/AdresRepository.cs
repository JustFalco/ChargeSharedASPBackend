using ChargeSharedProto2.Data.Contexts;
using ChargeSharedProto2.Models;
using Microsoft.EntityFrameworkCore;

namespace ChargeSharedProto2.Data.Repositories
{
    public class AdresRepository : IAdresRepository
    {
        private readonly ChargeShareDbContext _context;
        public AdresRepository(ChargeShareDbContext context)
        {
            _context = context;
        }

        /*
         * Get all adresses
         */
        public async Task<IEnumerable<UserAdres>> GetAllAsync()
        {
            return _context.Adresses;
        }

        public bool CheckIfAdressExists(string postalcode, string housnumber, string houseAddition, string country)
        {
            if (_context.Adresses.Where(a => (a.Country == country) && (a.HouseNumber == housnumber) && (a.HouseAddition == houseAddition) && (a.PostalCode == postalcode)).ToList().FirstOrDefault(defaultValue: null) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<UserAdres> SaveAdresAsync(UserAdres userAdres)
        {
            if (userAdres == null) throw new ArgumentNullException();

            _context.Adresses.Add(userAdres);
            await _context.SaveChangesAsync();

            return userAdres;
        }

        public UserAdres GetAdresByDataAsync(string postalcode, string housnumber, string houseAddition,
            string country)
        {
            return _context.Adresses.Where(a => a.Country == country).Where(a => a.HouseNumber == housnumber)
                .Where(a => a.HouseAddition == houseAddition).Where(a => a.PostalCode == postalcode)
                .First();
        }

        public UserAdres? GetAdresByPostalForAutoFill(string postalCode)
        {
            return _context.Adresses.Where(a => a.PostalCode == postalCode).ToList().FirstOrDefault(defaultValue: null);
        }
    }
}
