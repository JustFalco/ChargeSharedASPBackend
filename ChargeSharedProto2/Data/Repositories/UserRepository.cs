using ChargeSharedProto2.Data.Contexts;
using ChargeSharedProto2.Models;

namespace ChargeSharedProto2.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return _context.Users;
        }

    }
}
