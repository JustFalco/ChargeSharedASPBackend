using ChargeSharedProto2.Data.Contexts;
using ChargeSharedProto2.Models;
using Microsoft.EntityFrameworkCore;

namespace ChargeSharedProto2.Data.Repositories
{
    public class ChargeStationRepository : IChargeStationRepository
    {
        private readonly ChargeShareDbContext _context;
        public ChargeStationRepository(ChargeShareDbContext context)
        {
            _context = context;
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
    }
}
