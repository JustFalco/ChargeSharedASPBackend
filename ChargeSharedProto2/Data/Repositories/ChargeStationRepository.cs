using ChargeSharedProto2.Data.Contexts;
using ChargeSharedProto2.Data.DTOs;
using ChargeSharedProto2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChargeSharedProto2.Data.Repositories
{
    public class ChargeStationRepository : IChargeStationRepository
    {
        private readonly ChargeShareDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChargeStationRepository(ChargeShareDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /*
         * Get all adresses
         */
        public async Task<IEnumerable<ChargeStation>> GetAllAsync()
        {
            return _context.Chargers.Include(c => c.Adres);
        }

        public ChargeStation GetChargerById(int id)
        {
            return _context.Chargers.Where(c => c.Id == id).Include(c => c.Adres).Include(c => c.Owner).First();
        }

        public async Task<ChargeStation> SaveChargeStationAsync(ChargeStation chargeStation, string email)
        {
            try
            {
                if (chargeStation == null) throw new ArgumentNullException();

                _context.Chargers.Add(chargeStation);
                
                ApplicationUser chargerOwner = await _userManager.FindByEmailAsync(email);

                chargeStation.OwnerId = chargerOwner!.Id;

                await _context.SaveChangesAsync();

                return chargeStation;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<List<ChargeStation>> GetAllChargersFromUserAsync(string email)
        {
            //TODO add checks
            var result = _context.Chargers.Where(c => c.Owner.Email == email).ToList();

            return result;
        }

        public void RemoveChargestationById(int id)
        {
            //TODO add checks
            _context.Chargers.Where(c => c.Id == id).ExecuteDelete();
        }

        public async Task<IEnumerable<ChargeStation>> GetAllWithFilter(Filters filters)
        {
            var allChargers = await GetAllAsync();
            
            var result = allChargers;

            if (filters.useFilters)
            {
                if (!string.IsNullOrEmpty(filters.adresPostalCity))
                {
                    result = result.Where(c =>
                        c.Adres.PostalCode == filters.adresPostalCity || c.Adres.Street == filters.adresPostalCity ||
                        c.Adres.City == filters.adresPostalCity);
                }
                
                if (filters.maxPrice >= 0)
                {
                    result = result.Where(c => c.PricePerHour <= filters.maxPrice);
                }

                if (filters.chargerType != ChargerType.Null)
                {
                    result = result.Where(c => c.ChargerType == filters.chargerType);
                }  
            }
            
                       
            return result;
        }
    }
}
