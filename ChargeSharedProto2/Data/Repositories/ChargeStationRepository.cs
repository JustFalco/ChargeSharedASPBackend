using ChargeSharedProto2.Data.Contexts;
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
            return _context.Chargers;
        }

        public ChargeStation GetChargerById(int id)
        {
            return _context.Chargers.Where(c => c.Id == id).Include(c => c.Adres).First();
        }

        public async Task<ChargeStation> SaveChargeStationAsync(ChargeStation chargeStation)
        {
            if (chargeStation == null) throw new ArgumentNullException();

            _context.Chargers.Add(chargeStation);
            await _context.SaveChangesAsync();

            return chargeStation;
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
    }
}
